using BU.OnlineShop.FileService.Domain.Shared.FileInformations;
using BU.OnlineShop.Shared.Entities;
using System.Diagnostics.CodeAnalysis;

namespace BU.OnlineShop.FileService.Domain.FileInformations
{
    public class FileInformation : BaseEntity
    {
        [NotNull]
        public string Name { get; protected set; }

        [NotNull]
        public string MimeType { get; protected set; }

        [NotNull]
        public int Size { get; protected set; }

        [NotNull]
        public byte[] Content { get; protected set; }

        protected FileInformation() { }

        internal FileInformation(
            [NotNull] Guid id,
            [NotNull] string name, 
            [NotNull] string mimeType, 
            [NotNull] int size,
            [NotNull] byte[] content) 
        {
            Id = id;

            SetName(name);
            SetMimeType(mimeType);
            SetSize(size);
            SetContent(content);

        }

        public void SetName([NotNull] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{name} can not be null, empty or white space!");
            }

            if (name.Length >= FileInformationConsts.MaxNameLength)
            {
                throw new ArgumentException($"Product code can not be longer than {FileInformationConsts.MaxNameLength}");
            }

            Name = name;
        }

        protected void SetMimeType([NotNull] string mimeType)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentException($"{mimeType} can not be null, empty or white space!");
            }

            if (mimeType.Length >= FileInformationConsts.MaxMimeTypeLength)
            {
                throw new ArgumentException($"Product code can not be longer than {FileInformationConsts.MaxMimeTypeLength}");
            }

            MimeType = mimeType;
        }

        protected void SetSize([NotNull] int size)
        {
            if (size < 0)
            {
                throw new ArgumentException($"{nameof(size)} can not be less than 0!");
            }

            if (size > FileInformationConsts.MaxSizeLength) // 2 GB
            {
                throw new ArgumentException($"{nameof(size)} can not be greater than {FileInformationConsts.MaxSizeLength} bytes.");
            }


            Size = size;
        }

        protected void SetContent([NotNull] byte[] content)
        {
            if (content.Length < 0)
            {
                throw new ArgumentException($"{nameof(content)} can not be less than 0!");
            }

            if (content.Length > FileInformationConsts.MaxContentLength) // 2 GB
            {
                throw new ArgumentException($"{nameof(content)} can not be greater than {FileInformationConsts.MaxContentLength} bytes.");
            }

            Content = content;
        }
    }
}
