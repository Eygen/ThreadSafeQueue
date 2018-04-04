using System;
using System.Threading;

namespace ThreadSafeQueue
{
  class Program
  {
    static TestQueue<int> queue = new TestQueue<int>();

    static void Main(string[] args)
    {                        
      Thread popThread = new Thread(new ThreadStart(Dequeue));
      popThread.Start();      
      for (var i = 1; i <= 10; i++)
      {
        queue.Push(i);
      }  
      
      foreach (int item in queue)
      {
        Console.WriteLine(item);
      }
      Dequeue();
      Console.ReadKey();
    }

    public static void Dequeue()
    {
      int popNumber = queue.Pop();
      Console.WriteLine($"Извлеченный элемент: {popNumber}");
    }
  }
}
