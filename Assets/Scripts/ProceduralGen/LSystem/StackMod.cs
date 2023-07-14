using System.Collections.Generic;

// Delegate for modifying a stack
public delegate Stack<T> StackMod<T>(T item, Stack<T> stack);