using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadSafeQueue
{
  public class TestQueue<T> : IEnumerable<T>
  {
    Node<T> Head;
    Node<T> Tail;
    volatile int Count;
    object Locker = new object();
    static Semaphore QueueSemaphore = new Semaphore(0, Int32.MaxValue);

    public void Push(T data)
    {
      lock (Locker)
      {
        Node<T> node = new Node<T>(data);
        Node<T> tempNode = Tail;
        Tail = node;
        if (Count == 0)
          Head = Tail;
        else
          tempNode.Next = Tail;
        Count++;
        QueueSemaphore.Release();
      }
    }

    public T Pop()
    {
      QueueSemaphore.WaitOne();
      lock (Locker)
      {
        T data = Head.Data;
        Head = Head.Next;
        Count++;
        return data;
      }
    }

    public IEnumerator<T> GetEnumerator()
    {
      Node<T> current = Head;
      while (current != null)
      {
        yield return current.Data;
        current = current.Next;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable)this).GetEnumerator();
    }
  }

  public class Node<T>
  {
    public Node(T data)
    {
      Data = data;
    }
    public T Data { get; set; }

    public Node<T> Next { get; set; }
  }
}
