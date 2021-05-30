//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PraktikaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class TennisPlayers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TennisPlayers()
        {
            this.Match_Tennis_Player = new HashSet<Match_Tennis_Player>();
            this.Match_Progress = new HashSet<Match_Progress>();
        }
    
        public int ID_TennisPlayers { get; set; }
        [Required]
        [Display (Name = "Фамилия")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Страна")]
        public string Country { get; set; }
        [Required]
        [Display(Name = "Возраст")]
        public int Age { get; set; }
        [Required]
        [Display(Name = "Рейтинг")]
        public int Rating { get; set; }
        [Required]
        [Display(Name = "Ведущая рука")]
        public string Hand { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Match_Tennis_Player> Match_Tennis_Player { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Match_Progress> Match_Progress { get; set; }
    }
}
