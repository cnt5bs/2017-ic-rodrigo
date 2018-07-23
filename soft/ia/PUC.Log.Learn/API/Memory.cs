using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parameters;
using Repository;
using Newtonsoft.Json.Linq;
using Repository.Learn;

namespace Machine
{
    public static class Memory
    {
        public static void InsertSingleMemory(object memory)
        {
            InsertSingleMemory(memory, null);
        }

        public static void InsertSingleMemory(object memory, string environment)
        {
            Understand understand = new Understand()
            {
                memory = memory
            };
            MemoryDB memoryDB = new MemoryDB();
            memoryDB.InsertMemory(understand);
            memoryDB.InsertIndirectUnderstanding(memory, memory, environment);
        }

        public static void InsertRelatedMemories(object responsible, object fact, object environment)
        {
            MemoryDB memoryDB = new MemoryDB();
            memoryDB.InsertMemory(new Understand() { memory = responsible });
            memoryDB.InsertMemory(new Understand() { memory = fact });
            memoryDB.InsertIndirectUnderstanding(responsible, fact, environment);
        }
        public static void InsertRelatedMemories(object responsible, string fact, object environment)
        {
            MemoryDB memoryDB = new MemoryDB();
            memoryDB.InsertMemory(new Understand() { memory = responsible });
            memoryDB.InsertMemory(new Understand() { memory = fact });
            memoryDB.InsertIndirectUnderstanding(responsible, fact, environment);
        }

        public static List<JObject> ListAllMemories()
        {
            MemoryDB memoryDB = new MemoryDB();
            return memoryDB.ListAllMemories();
        }
    }
}
