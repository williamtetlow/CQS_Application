﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICommandHandler <in TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}
