using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Skyray.EDX.Common.Component
{
    /// <summary>
    /// 线程安全队列
    /// </summary>
    class ThreadSafeQueue
    {
        #region 构造函数
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.Queue<T> class
        //     that is empty and has the default initial capacity.
        public ThreadSafeQueue()
        {
            m_queue = new Queue<Byte[]>();
        }
        #endregion

        #region 构造函数
        //
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.Queue<T> class
        //     that is empty and has the specified initial capacity.
        //
        // Parameters:
        //   capacity:
        //     The initial number of elements that the System.Collections.Generic.Queue<T>
        //     can contain.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     capacity is less than zero.
        public ThreadSafeQueue(int capacity)
        {
            m_queue = new Queue<Byte[]>(capacity);
        }
        #endregion

        #region 入队列
        //
        // Summary:
        //     Adds an object to the end of the System.Collections.Generic.Queue<T>.
        //
        // Parameters:
        //   item:
        //     The object to add to the System.Collections.Generic.Queue<T>. The value can
        //     be null for reference types.
        public void Enqueue(Byte[] buf)
        {
            lock(m_lockobj)
            {
                m_queue.Enqueue(buf);
            }
        }
        #endregion

        #region 出队列
        //
        // Summary:
        //     Removes and returns the object at the beginning of the System.Collections.Generic.Queue<T>.
        //
        // Returns:
        //     The object that is removed from the beginning of the System.Collections.Generic.Queue<T>.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Collections.Generic.Queue<T> is empty.
        public Byte[] Dequeue()
        {
            lock(m_lockobj)
            {
                return m_queue.Dequeue();
            }
        }
        #endregion

        #region 查看队列头，但不出队列
        //
        // Summary:
        //     Returns the object at the beginning of the System.Collections.Generic.Queue<T>
        //     without removing it.
        //
        // Returns:
        //     The object at the beginning of the System.Collections.Generic.Queue<T>.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Collections.Generic.Queue<T> is empty.
        public Byte[] Peak()
        {
            lock(m_lockobj)
            {
                return m_queue.Peek();
            }
        }
        #endregion

        #region 清空队列
        // Summary:
        //     Removes all objects from the System.Collections.Generic.Queue<T>.
        public void Clear()
        {
            lock(m_lockobj)
            {
                m_queue.Clear();
            }
        }
        #endregion

        #region 队列载荷
        public int Count 
        {
            get
            {
                return m_queue.Count;
            }
        }
        #endregion

        private Queue<Byte[]> m_queue;              // Byte[]队列
        private object m_lockobj = new object();    // 用于lock指令
    }
}
