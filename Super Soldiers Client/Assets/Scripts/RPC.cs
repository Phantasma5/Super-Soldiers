using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RPC
{
    //methods with this attribute will be able to be called remotely; must be in a HasRPCs class;
    [System.AttributeUsage(AttributeTargets.Method)]
    public class RPCMethod : Attribute
    {

    }

    //classes with this attribute will be scanned for methods that can be called remotely;
    [System.AttributeUsage(AttributeTargets.Class)]
    public class HasRPCs : Attribute
    {

    }

    public struct HelperFunctions
    {
        //check a method for the RPC flag
        public static bool CheckMethod(MethodInfo methodInfo)
        {
            return Attribute.IsDefined(methodInfo, typeof(RPC));
        }
        //check a class for the HasRPCs flag
        public static bool CheckClass(Type type)
        {
            return Attribute.IsDefined(type, typeof(HasRPCs));
        }
        //Go through a list of components and return the ones that have the HasRPCs flag
        public static Component[] FilterScripts(Component[] components)
        {
            List<Component> returnMe = new List<Component>();
            foreach (Component component in components)
            {
                if (CheckClass(component.GetType()))
                {
                    returnMe.Add(component);
                }
            }
            return returnMe.ToArray();
        }
    }
}