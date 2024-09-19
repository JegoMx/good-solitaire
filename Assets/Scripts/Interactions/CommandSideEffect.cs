using System;

namespace Game.Interactions
{
    public sealed class CommandSideEffect
    {
        private readonly Action _undoAction;

        public Action UndoAction => _undoAction;

        public CommandSideEffect(Action undoAction)
        {
            _undoAction = undoAction;
        }
    }
}