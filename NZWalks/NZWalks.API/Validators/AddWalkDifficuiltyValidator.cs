using FluentValidation;

namespace NZWalks.API.Validators
{
    public class AddWalkDifficuiltyValidator:AbstractValidator<Models.DTO.AddWalkDifficuiltyRequest>
    {
        public AddWalkDifficuiltyValidator()
        {
            RuleFor(x => x.Code).NotEmpty();

        }
    }
}
