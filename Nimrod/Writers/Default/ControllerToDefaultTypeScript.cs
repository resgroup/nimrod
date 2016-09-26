﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Default
{
    public class ControllerToDefaultTypeScript : ControllerToTypeScript
    {
        public override bool NeedNameSpace => true;

        public ControllerToDefaultTypeScript(TypeScriptType type, bool strictNullCheck) : base(type, strictNullCheck) { }

        protected override IEnumerable<string> GetHeader() => new[] {
            $"namespace {this.Type.Namespace} {{"
        };

        protected override IEnumerable<string> GetFooter() => new[] {
            $"service('serverApi.{ServiceName}', {ServiceName});",
            $"}}"
        };
    }
}
