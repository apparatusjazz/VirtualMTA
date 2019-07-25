using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T, K> 
{
    Dictionary<T, K> items;
    OrderingConvention convention; 
    public Int32 Count
    {
        get { return items.Count; }
    }
    public OrderingConvention Convention
    {
        get { return Convention; }
        set { Convention = value; }
    }
    public void Sort()
    {
        if(this.convention == OrderingConvention.None)
        {
            throw new MissingFieldException();
        }
        if (this.convention == OrderingConvention.Min)
        {
            var sortedDict = from entry in items orderby entry.Value ascending select entry;
            items = sortedDict.ToDictionary(x => x.Key, x => x.Value);
        }
        if(this.convention == OrderingConvention.Max)
        {
            var sortedDict = from entry in items orderby entry.Value descending select entry;
            items = sortedDict.ToDictionary(x => x.Key, x => x.Value);
        }
    }
    public void PrintValues()
    {
        foreach(KeyValuePair<T,K> p in items)
        {
            System.Console.WriteLine("Key = {0}, Value ={1}", p.Key, p.Value);
        }
    }
    public PriorityQueue()
    {
        this.convention = OrderingConvention.None;
    }
    public PriorityQueue(OrderingConvention myconvention)
    {
        this.convention = myconvention;
    }
    public void Add(T t, K k)
    {
        items.Add(t, k);
    }
    public void Remove(T t)
    {
        items.Remove(t);
    }
}
public enum OrderingConvention
{
    None, 
    Min,
    Max
}