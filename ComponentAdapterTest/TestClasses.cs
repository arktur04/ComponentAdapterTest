using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using VarDictionaryClasses;

namespace TestClasses
{
    public class DbMock
    {
        public VarDictionary dict {get; set;}
        public Timer timer;
        private float i = 0;
        private const int period = 60;
        public Mapper mapper;

        public DbMock()
        {
            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = true;
          //  timer.Start();
        }

        private void timer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            i++;
            if(dict != null)
            {
                dict["FloatVar2"].value = Math.Sin((i / period) * 2 * Math.PI).ToString();
            };
            mapper.applyMapping();
        }
    }
}
