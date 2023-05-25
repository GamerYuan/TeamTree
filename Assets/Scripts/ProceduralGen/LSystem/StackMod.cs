using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Delegate for modifying a stack
public delegate Stack<T> StackMod<T>(T item, Stack<T> stack);