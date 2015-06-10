using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OurHelper
{ 
    /// <summary>
    /// 频率恒定计时器
    /// </summary>
    public class FixedRateTimer
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="start">启动后延迟多久开始执行</param>
        /// <param name="interval">下次执行距离这次执行开始的时间</param>
        /// <param name="action">任务委托</param>
        /// <param name="state">任务状态参数</param>
        /// <param name="concurrent">是否允许并行
        /// <remarks>如果允许并行，同一时间可能有几个任务同时执行;
        /// 如果不允许,当新任务发现上次任务正在运行时，将忽略此次任务</remarks>
        /// </param>
        public FixedRateTimer(TimeSpan start, TimeSpan interval, TimerCallback action,
            object state = null, bool concurrent = false)
        {
            _action = action;
            _isConcurrent = concurrent;

            _start = start;
            _interval = interval;
            _state = state;
        }

        private void TimerAction(object s)
        {
            try
            {
                //增加正在执行任务个数
                Interlocked.Increment(ref _jobCount);
                if (!_isConcurrent)
                {
                    //:任务无法并行执行，所以必须串行化

                    //当无任务执行(_isRunning==0)时更改状态为运行(_isRunning=0),
                    //然后开始执行新任务,因为代码可能多线程执行，所以用原子操作
                    if (Interlocked.CompareExchange(ref _isRunning, 1, 0) == 0)
                    {
                        _action(s);
                        //更改状态为无任务运行
                        Interlocked.Exchange(ref _isRunning, 0);
                    }
                }
                else
                {
                    //无串行约束，直接执行
                    _action(s);
                }

                //减少正在执行任务个数
                Interlocked.Decrement(ref _jobCount);
            }
            catch{ 
                
            }
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            _isRunning = 0;
            _timer = new Timer(TimerAction, _state, _start, _interval);
        }

        /// <summary>
        /// 结束计时
        /// </summary>
        public void Stop()
        {
            _timer.Dispose();
            WaitUtilFinish();
            _timer = null;
            _isRunning = 0;
        }

        /// <summary>
        /// 等待已经开始执行的任务执行完
        /// </summary>
        private void WaitUtilFinish()
        {
            while (_jobCount != 0)
            {
                Thread.Sleep(_interval);
            }
        }

        //用来在不允许并发时保持任务串行
        private int _isRunning = 0;
        private int _jobCount = 0;
        private bool _isConcurrent = false;

        private TimerCallback _action = null;
        private Timer _timer = null;

        private TimeSpan _start;
        private TimeSpan _interval;
        private Object _state;
    }
}
