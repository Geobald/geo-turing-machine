﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace Emulator
{
    public class Machine
    {
        private Code code;

        private Memory<char> memory;
        public Memory<char> Memory { get { return memory; } }

        private string state;
        public string State { get { return state; } }

        private int cycles;
        public int Cycles { get { return cycles; } }

        protected virtual void OnCycle(MachineEventArgs e)
        {
            EventHandler<MachineEventArgs> ev = Cycle;
            if (ev != null)
                ev(this, e);
        }

        public event EventHandler<MachineEventArgs> Cycle;

        /// <summary>
        /// Raises the Finish event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnFinish(MachineEventArgs e)
        {
            EventHandler<MachineEventArgs> ev = Finish;
            if (ev != null)
                ev(this, e);
        }
        public event EventHandler<MachineEventArgs> Finish;

        #region Constructors

        public Machine(List<char> memory)
        {
            this.memory = new Memory<char>(memory, '_');
        }

        public Machine(List<char> memory, string[] prog) : this(memory)
        {
            code = new Code(prog);
        }

        #endregion
        #region Runtime

        /// <summary>
        /// Starts the computation
        /// </summary>
        /// <param name="state">The starting program state</param>
        /// <param name="delay">The delay to apply to each cycle of the machine</param>
        public void Run(string state = "*", int delay = 0)
        {
            this.state = state;
            string[] command;
            while (state != "halt")
            {
                try
                {
                    command = code.Match(state, memory.Read().ToString());
                }
                catch (KeyNotFoundException e)
                {
                    Console.Error.WriteLine(e.Message);
                    break;
                }

                if (command[0] != "*")
                    memory.Write(Convert.ToChar(command[0]));

                if (command[1] == "r")
                    memory.MoveRight();
                else if (command[1] == "l")
                    memory.MoveLeft();

                state = command[2];

                OnCycle(new MachineEventArgs(memory, state, cycles));
                cycles++;

                if (delay > 0)
                    Thread.Sleep(delay);
            }
            OnFinish(new MachineEventArgs(memory, state, cycles));
        }
        #endregion
    }

    public class MachineEventArgs : EventArgs
    {
        private Memory<char> memory;
        public Memory<char> Memory { get { return memory; } }

        private string state;
        public string State { get { return state; } }

        private int cycles;
        public int Cycles { get { return cycles; } }

        public MachineEventArgs(Memory<char> memory, string state, int cycles)
        {
            this.memory = memory;
            this.state = state;
            this.cycles = cycles;
        }
    }
}
