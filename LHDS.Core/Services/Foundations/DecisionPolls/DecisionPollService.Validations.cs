// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;

namespace LHDS.Core.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollService
    {
        private async ValueTask ValidateDecisionPollOnAddAsync(DecisionPoll decisionPoll)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
            (Rule: await IsNotRecentAsync(decisionPoll.CreatedDate), Parameter: nameof(DecisionPoll.CreatedDate)));
        }

        private static void ValidateDecisionPollIsNotNull(DecisionPoll decisionPoll)
        {
            if (decisionPoll is null)
            {
                throw new NullDecisionPollException(message: "DecisionPoll is null.");
            }
        }

        private async ValueTask<dynamic> IsNotRecentAsync(DateTimeOffset date) => new
        {
            Condition = await IsDateNotRecentAsync(date),
            Message = "Date is not recent"
        };

        private async ValueTask<bool> IsDateNotRecentAsync(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");


            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDecisionPollException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDecisionPollException.ThrowIfContainsErrors();
        }
    }
}
