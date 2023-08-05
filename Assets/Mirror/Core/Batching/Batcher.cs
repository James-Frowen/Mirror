// batching functionality encapsulated into one class.
// -> less complexity
// -> easy to test
//
// IMPORTANT: we use THRESHOLD batching, not MAXED SIZE batching.
// see threshold comments below.
//
// includes timestamp for tick batching.
// -> allows NetworkTransform etc. to use timestamp without including it in
//    every single message
using System;
using System.Collections.Generic;

namespace Mirror
{
    public class Batcher
    {
        // batching threshold instead of max size.
        // -> small messages are fit into threshold sized batches
        // -> messages larger than threshold are single batches
        //
        // in other words, we fit up to 'threshold' but still allow larger ones
        // for two reasons:
        // 1.) data races: skipping batching for larger messages would send a
        //     large spawn message immediately, while others are batched and
        //     only flushed at the end of the frame
        // 2) timestamp batching: if each batch is expected to contain a
        //    timestamp, then large messages have to be a batch too. otherwise
        //    they would not contain a timestamp
        readonly int threshold;
        private readonly int channel;
        private readonly NetworkConnection connection;

        // TimeStamp header size. each batch has one.
        public const int TimestampSize = sizeof(double);

        // Message header size. each message has one.
        public static int MessageHeaderSize(int messageSize) =>
            Compression.VarUIntSize((ulong)messageSize);

        // maximum overhead for a single message.
        // useful for the outside to calculate max message sizes.
        public static int MaxMessageOverhead(int messageSize) =>
            TimestampSize + MessageHeaderSize(messageSize);

        // full batches ready to be sent.
        // DO NOT queue NetworkMessage, it would box.
        // DO NOT queue each serialization separately.
        //        it would allocate too many writers.
        //        https://github.com/vis2k/Mirror/pull/3127
        // => best to build batches on the fly.
        //readonly Queue<NetworkWriterPooled> batches = new Queue<NetworkWriterPooled>();

        // current batch in progress
        readonly NetworkWriter batch = new NetworkWriter();
        bool hasBatch;

        public Batcher(int threshold, int channel, NetworkConnection connection)
        {
            this.threshold = threshold;
            this.channel = channel;
            this.connection = connection;
        }

        // add a message for batching
        // we allow any sized messages.
        // caller needs to make sure they are within max packet size.
        public void AddMessage(ArraySegment<byte> message, double timeStamp)
        {
            // predict the needed size, which is varint(size) + content
            int headerSize = Compression.VarUIntSize((ulong)message.Count);
            int neededSize = headerSize + message.Count;

            // when appending to a batch in progress, check final size.
            // if it expands beyond threshold, then we should finalize it first.
            // => less than or exactly threshold is fine.
            //    GetBatch() will finalize it.
            // => see unit tests.
            if (hasBatch &&
                batch.Position + neededSize > threshold)
            {
                Flush();
            }

            // initialize a new batch if necessary
            if (!hasBatch)
            {
                hasBatch = true;
                // borrow from pool. we return it in GetBatch.
                //batch = NetworkWriterPool.Get();

                // write timestamp first.
                // -> double precision for accuracy over long periods of time
                // -> batches are per-frame, it doesn't matter which message's
                //    timestamp we use.
                batch.WriteDouble(timeStamp);
            }

            // add serialization to current batch. even if > threshold.
            // -> we do allow > threshold sized messages as single batch
            // -> WriteBytes instead of WriteSegment because the latter
            //    would add a size header. we want to write directly.
            //
            // include size prefix as varint!
            // -> fixes NetworkMessage serialization mismatch corrupting the
            //    next message in a batch.
            // -> a _lot_ of time was wasted debugging corrupt batches.
            //    no easy way to figure out which NetworkMessage has a mismatch.
            // -> this is worth everyone's sanity.
            // -> varint means we prefix with 1 byte most of the time.
            // -> the same issue in NetworkIdentity was why Mirror started!
            Compression.CompressVarUInt(batch, (ulong)message.Count);
            batch.WriteBytes(message.Array, message.Offset, message.Count);
        }

        public void Flush()
        {
            if (hasBatch)
            {
                connection.SendToTransport(batch.ToArraySegment(), channel);
                batch.Reset();
                hasBatch = false;

                //Transport.active.ServerLateUpdate();
            }
        }
    }
}
