using System;
using System.Collections.Generic;
using System.Text;

namespace ImageFilters
{
    class Heap
    {
        int[] _arr;
        int _size;

        public Heap(int[] arr)
        {
            _arr = arr;
            _size = arr.Length;
        }
        public void BuildMinHeap()
        {
            for (int i = (_size - 1) / 2; i >= 0; i--)
            {
                MinHeapify(i, _size - 1);
            }
        }

        public void BuildMaxHeap()
        {
            for (int i = (_size - 1) / 2; i >= 0; i--)
            {
                MaxHeapify(i, _size - 1);
            }
        }

        public void MinHeapify(int i, int maxIndex)
        {
            int leftChild = ChildLeft(i);
            int rightChild = ChildRight(i);

            int min = i;

            if (leftChild <= maxIndex && _arr[min] > _arr[leftChild])
            {
                min = leftChild;
            }
            if (rightChild <= maxIndex && _arr[min] > _arr[rightChild])
            {
                min = rightChild;
            }

            if (min <= maxIndex && min != i)
            {
                Swap(i, min);
                MinHeapify(min, maxIndex);
            }
        }

        public void MaxHeapify(int i, int maxIndex)
        {
            int leftChild = ChildLeft(i);
            int rightChild = ChildRight(i);

            int max = i;

            if (leftChild <= maxIndex && _arr[max] < _arr[leftChild])
            {
                max = leftChild;
            }
            if (rightChild <= maxIndex && _arr[max] < _arr[rightChild])
            {
                max = rightChild;
            }

            if (max <= maxIndex && max != i)
            {
                Swap(i, max);
                MaxHeapify(max, maxIndex);
            }
        }

        public void Swap(int x, int y)
        {
            int temp = _arr[x];
            _arr[x] = _arr[y];
            _arr[y] = temp;
        }

        public int Size()
        {
            return _size;
        }

        public int ElementAt(int i)
        {
            return _arr[i];
        }

        private int ChildLeft(int index)
        {
            return (index * 2) + 1;
        }
        private int ChildRight(int index)
        {
            return (index * 2) + 2;
        }
        private int Parent(int index)
        {
            return index / 2;
        }
    }
}
