using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class NeedToAssignComponentAttribute : PropertyAttribute {
}

//[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
//public class MethodButtonAttribute : PropertyAttribute {

