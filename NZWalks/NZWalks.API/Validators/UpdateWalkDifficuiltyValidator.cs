using FluentValidation;

namespace NZWalks.API.Validators
{
    public class UpdateWalkDifficuiltyValidator:AbstractValidator<Models.DTO.UpdateWalkDifficuiltyRequest>
    {
        public UpdateWalkDifficuiltyValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
