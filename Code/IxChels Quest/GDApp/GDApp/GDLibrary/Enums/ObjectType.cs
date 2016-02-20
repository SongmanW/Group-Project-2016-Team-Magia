﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public enum ObjectType : sbyte
    {
        //drawn objects
        Prop,               //a health pickup, an interactive object like a door
        Decorator,          //a chair, a table
        Player,             //you
        NonPlayerCharacter, //enemy

        //cameras
        FirstPerson, 
        ThirdPerson, 
        Rail, 
        Track, 
        Fixed, 
        Security
    }
}