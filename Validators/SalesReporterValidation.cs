using FluentValidation;
using Reports;

namespace Validators
{
    public class SalesReporterValidation : AbstractValidator<SalesReporter>
    {
        public SalesReporterValidation()
        {
            RuleFor(salesReport => salesReport.SimpleBill).NotNull();
        }
    }
}
