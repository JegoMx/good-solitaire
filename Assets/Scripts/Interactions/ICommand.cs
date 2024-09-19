using System;

namespace Game.Interactions
{
    public interface ICommand
    {
        public void Execute(out bool success);
        public void Undo();
    }
}