﻿using System;
using System.Collections.Generic;

namespace HeadLibrarian
{
	public delegate void UndoRedoStackEventHandler( Int32 currentUndoStackSize, Int32 currentRedoStackSize );

	public class UndoRedoStack
	{
		public void Push( IUndoRedoAction action )
		{
			if ( action == null )
			{
				throw new ArgumentNullException( nameof( action ) );
			}

			lock ( _stackLock )
			{
				_redoStack.Clear();
				_undoStack.Push( action );
			}
		}

		public Int32 UndoStackSize
		{
			get
			{
				lock ( _stackLock )
				{
					return _undoStack.Count;
				}
			}
		}

		public Int32 RedoStackSize
		{
			get
			{
				lock ( _stackLock )
				{
					return _redoStack.Count;
				}
			}
		}

		public void Undo()
		{
			lock ( _stackLock )
			{
				if ( _undoStack.Count == 0 )
				{
					return;
				}

				IUndoRedoAction action = _undoStack.Pop();
				action.Undo();
				_redoStack.Push( action );

				UndoPerformed?.Invoke( _undoStack.Count, _redoStack.Count );
			}
		}

		public event UndoRedoStackEventHandler UndoPerformed;

		public void Redo()
		{
			lock ( _stackLock )
			{
				if ( _redoStack.Count == 0 )
				{
					return;
				}

				IUndoRedoAction action = _redoStack.Pop();
				action.Redo();
				_undoStack.Push( action );

				RedoPerformed?.Invoke( _undoStack.Count, _redoStack.Count );
			}
		}

		public event UndoRedoStackEventHandler RedoPerformed;

		readonly Object _stackLock = new Object();
		readonly Stack<IUndoRedoAction> _undoStack = new Stack<IUndoRedoAction>();
		readonly Stack<IUndoRedoAction> _redoStack = new Stack<IUndoRedoAction>();
	}
}