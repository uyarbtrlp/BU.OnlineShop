using JetBrains.Annotations;

namespace BU.OnlineShop.CatalogService.Categories
{
    public class Category
    {
        public Guid Id { get; protected set; }

        [NotNull]
        public string Name { get; protected set; }

        public string Description { get; protected set; }

        protected Category()
        {
            // Default constructor is needed for ORMs.
        }

        internal Category(
          Guid id,
          [NotNull] string name,
          string description = null)
        {

            Id = id;
            SetName(name);
            SetDescription(description);
        }

        public void SetName([NotNull] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{name} can not be null, empty or white space!");
            }

            if (name.Length >= CategoryConsts.MaxNameLength)
            {
                throw new ArgumentException($"Product name can not be longer than {CategoryConsts.MaxNameLength}");
            }

            Name = name;
        }
        public void SetDescription(string description)
        {
            if (!string.IsNullOrEmpty(description)){

                if (description.Length >= CategoryConsts.MaxDescriptionLength)
                {
                    throw new ArgumentException($"Product name can not be longer than {CategoryConsts.MaxDescriptionLength}");
                }

                Description = description;
            }
            else
            {
                Description = null;
            }

        }
    }
}
