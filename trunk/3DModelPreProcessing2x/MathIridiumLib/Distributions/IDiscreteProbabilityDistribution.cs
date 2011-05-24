#region Math.NET Iridium (LGPL) by Ruegg
// Math.NET Iridium, part of the Math.NET Project
// http://Iridium.opensourcedotnet.info
//
// Copyright (c) 2002-2008, Christoph R�egg, http://christoph.ruegg.name
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
#endregion

using System;

namespace Iridium.Numerics.Distributions
{
    /// <summary>
    /// Discrete probability distribution, providing distribution properties and functions.
    /// </summary>
    public interface IDiscreteProbabilityDistribution
    {
        /// <summary>
        /// Discrete probability mass function (pmf) of this probability distribution.
        /// </summary>
        double ProbabilityMass(int x);

        /// <summary>
        /// Continuous cumulative distribution function (cdf) of this probability distribution.
        /// </summary>
        double CumulativeDistribution(double x);

        /// <summary>
        /// The expected value of a random variable with this probability distribution.
        /// </summary>
        double Mean
        {
            get;
        }

        /// <summary>
        /// Average of the squared distances to the expected value of a random variable with this probability distribution.
        /// </summary>
        double Variance
        {
            get;
        }

        /// <summary>
        /// The value separating the lower half part from the upper half part of a random variable with this probability distribution.
        /// </summary>
        int Median
        {
            get;
        }

        /// <summary>
        /// Lower limit of a random variable with this probability distribution.
        /// </summary>
        int Minimum
        {
            get;
        }

        /// <summary>
        /// Upper limit of a random variable with this probability distribution.
        /// </summary>
        int Maximum
        {
            get;
        }

        /// <summary>
        /// Measure of the asymmetry of this probability distribution.
        /// </summary>
        double Skewness
        {
            get;
        }
    }
}
