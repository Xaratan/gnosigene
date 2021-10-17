//Copyright 2021 Xaratan LLC
//Released under the MIT License (see LICENSE.txt)

using System;
using System.Collections.Generic;
using System.Text;

namespace Gnosigene
{
    public class SingleNucleotidePolymorphism
    {
        /// <summary>
        /// Reference SNP Identifier (could be anything).
        /// </summary>
        public string ReferenceIdentifier { get; }

        /// <summary>
        /// Used to indicate which chromosome this SNP is located on.  
        /// </summary>
        public HumanChromosome Chromosome { get; }

        /// <summary>
        /// Position on <see cref="Chromosome"/> where the SNP is located.
        /// </summary>
        public long Postion { get; }

        /// <summary>
        /// The nucleotides at this polymorphism.
        /// </summary>
        public NucleotidePair Nucleotides { get; }

        public SingleNucleotidePolymorphism(string referenceIdentifier, HumanChromosome chromosome, long postion, NucleotidePair nucleotides)
        {
            ReferenceIdentifier = referenceIdentifier ?? throw new ArgumentNullException(nameof(referenceIdentifier));
            Chromosome = chromosome;
            Postion = postion;
            Nucleotides = nucleotides;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ReferenceIdentifier);
            sb.Append(" ");
            sb.Append(Nucleotides.ToDisplayString());
            sb.Append(" ");
            sb.Append(Chromosome.ToDisplayString());
            sb.Append("@");
            sb.Append(Postion);

            return sb.ToString();
        }
    }
}
