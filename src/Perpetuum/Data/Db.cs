using Perpetuum.Log;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;

namespace Perpetuum.Data
{
    public sealed class Db
    {
        public static Func<DbQuery> DbQueryFactory { get; set; }

        public static DbQuery Query()
        {
            return DbQueryFactory();
        }

        public static DbQuery Query(string commandText)
        {
            var stackTrace = new StackTrace();
            var callingFrame = stackTrace.GetFrame(1);  // 1 is the caller's frame
            var callingMethod = callingFrame.GetMethod();
            var parameters = "";
            callingMethod.GetParameters().ForEach(x =>
            {
                parameters += x.ParameterType.Name + " " + x.Name + ", ";
            });
            parameters = parameters.Trim(' ', ',');
            Logger.Error($"--------------------- {callingMethod.DeclaringType.FullName} -> {callingMethod.Name} ({parameters})");
            return DbQueryFactory().CommandText(commandText);
        }

        public static Task CreateTransactionAsync(Action<TransactionScope> action)
        {
            return Task.Run(() =>
            {
                using (var scope = CreateTransaction())
                {
                    action(scope);
                    scope.Complete();
                }
            }).LogExceptions();
        }

        public static TransactionScope CreateTransaction()
        {
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            });
        }
    }
}