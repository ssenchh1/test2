﻿using System.Collections.Generic;
using System.Linq;
using DI.App.Abstractions;
using DI.App.Services.PL.Commands;

namespace DI.App.Services.PL
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly Dictionary<int, ICommand> commands = new Dictionary<int, ICommand>();

        public CommandProcessor()
        {
            //создаем хранилище, с которым будут работать команды.
            var store = new UserStore(new InMemoryDatabaseService());

            var addUsers = new AddUserCommand(store);
            var listUsers = new ListUsersCommand(store);

            this.commands.Add(addUsers.Number, addUsers);
            this.commands.Add(listUsers.Number, listUsers);
        }

        public void Process(int number)
        {
            if (!this.commands.TryGetValue(number, out var command)) return;

            command.Execute();
        }

        public IEnumerable<ICommand> Commands => this.commands.Values.AsEnumerable();
    }
}