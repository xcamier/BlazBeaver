using System.ComponentModel.DataAnnotations;

namespace BlazBeaver.Data;

    public class RequirementEditionForm
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string TypeOfRequirement { get; set; } = string.Empty;

        public List<string> Components { get; set; } = new List<string>();
        public List<string> SoftwareUnits { get; set; } = new List<string>();
        public string SRAIds { get; set; } = string.Empty;
        public string CreatedInVersion { get; set; } = string.Empty;
        public string DeprecatedInVersion { get; set; } = string.Empty;
        public bool IsSafety { get; set; } = false;
        public string Description { get; set; } = string.Empty;
        public string Changelog { get; set; } = string.Empty;
        public string AssociatedProtocolsIds { get; set; } = string.Empty;
    }
