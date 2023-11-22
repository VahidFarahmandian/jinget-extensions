﻿namespace System.Runtime.CompilerServices;

/// <summary>
/// add support for 'required' keyword using latest lang version and .net standard 2.1
/// </summary>
public class CompilerFeatureRequiredAttribute(string name) : Attribute
{
}