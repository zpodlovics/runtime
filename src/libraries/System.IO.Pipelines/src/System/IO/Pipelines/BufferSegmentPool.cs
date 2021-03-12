// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;

namespace System.IO.Pipelines
{
    internal class BufferSegmentPool
    {
        private readonly int _maxSegmentPoolSize;
        private readonly ConcurrentQueue<BufferSegment> _pool = new ConcurrentQueue<BufferSegment>();

        public BufferSegmentPool(int maxSegmentPoolSize) => _maxSegmentPoolSize = maxSegmentPoolSize;

        public BufferSegment Rent()
        {
            if (_pool.TryDequeue(out BufferSegment? segment))
            {
                return segment;
            }
            return new BufferSegment();
        }

        public bool Return(BufferSegment segment)
        {
            if (_pool.Count > _maxSegmentPoolSize)
            {
                return false;
            }

            _pool.Enqueue(segment);
            return true;
        }
    }
}
