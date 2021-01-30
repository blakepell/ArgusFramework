using System;
using System.Collections.Generic;

namespace Argus.Data.Reporting
{
    /// <summary>
    /// Utilities for US States
    /// </summary>
    public class UsStates
    {
        //*********************************************************************************************************************
        //
        //             Class:  UsStates
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  03/27/2009
        //      Last Updated:  04/08/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// An enumeration of the US States.
        /// </summary>
        public enum State
        {
            Alabama,
            Alaska,
            Arizona,
            Arkansas,
            California,
            Colorado,
            Connecticut,
            Delaware,
            Florida,
            Georgia,
            Hawaii,
            Idaho,
            Illinois,
            Indiana,
            Iowa,
            Kansas,
            Kentucky,
            Louisiana,
            Maine,
            Maryland,
            Massachusetts,
            Michigan,
            Minnesota,
            Mississippi,
            Missouri,
            Montana,
            Nebraska,
            Nevada,
            NewHampshire,
            NewJersey,
            NewMexico,
            NewYork,
            NorthCarolina,
            NorthDakota,
            Ohio,
            Oklahoma,
            Oregon,
            Pennsylvania,
            RhodeIsland,
            SouthCarolina,
            SouthDakota,
            Tennessee,
            Texas,
            Utah,
            Vermont,
            Virginia,
            Washington,
            WestVirginia,
            Wisconsin,
            Wyoming
        }

        /// <summary>
        /// Returns a state's enum based off of the abbreviation passed in.
        /// </summary>
        /// <param name="stateString"></param>
        public static State StateFromString(string stateString)
        {
            switch (stateString.ToUpper().Trim())
            {
                case "AL":
                case "ALABAMA":
                    return State.Alabama;
                case "AK":
                case "ALASKA":
                    return State.Alaska;
                case "AZ":
                case "ARIZONA":
                    return State.Arizona;
                case "AR":
                case "ARKANSAS":
                    return State.Arkansas;
                case "CA":
                case "CALIFORNIA":
                    return State.California;
                case "CO":
                case "COLORADO":
                    return State.Colorado;
                case "CT":
                case "CONNECTICUT":
                    return State.Connecticut;
                case "DE":
                case "DELAWARE":
                    return State.Delaware;
                case "FL":
                case "FLORIDA":
                    return State.Florida;
                case "GA":
                case "GEORGIA":
                    return State.Georgia;
                case "HI":
                case "HAWAII":
                    return State.Hawaii;
                case "ID":
                case "IDAHO":
                    return State.Idaho;
                case "IL":
                case "ILLINOIS":
                    return State.Illinois;
                case "IN":
                case "INDIANA":
                    return State.Indiana;
                case "IA":
                case "IOWA":
                    return State.Iowa;
                case "KS":
                case "KANSAS":
                    return State.Kansas;
                case "KY":
                case "KENTUCKY":
                    return State.Kentucky;
                case "LA":
                case "LOUISIANA":
                    return State.Louisiana;
                case "ME":
                case "MAINE":
                    return State.Maine;
                case "MD":
                case "MARYLAND":
                    return State.Maryland;
                case "MA":
                case "MASSACHUSETTS":
                    return State.Massachusetts;
                case "MI":
                case "MICHIGAN":
                    return State.Michigan;
                case "MN":
                case "MINNESOTA":
                    return State.Minnesota;
                case "MS":
                case "MISSISSIPPI":
                    return State.Mississippi;
                case "MO":
                case "MISSOURI":
                    return State.Missouri;
                case "MT":
                case "MONTANA":
                    return State.Montana;
                case "NE":
                case "NEBRASKA":
                    return State.Nebraska;
                case "NV":
                case "NEVADA":
                    return State.Nevada;
                case "NH":
                case "NEW HAMPSHIRE":
                    return State.NewHampshire;
                case "NJ":
                case "NEW JERSEY":
                    return State.NewJersey;
                case "NM":
                case "NEW MEXICO":
                    return State.NewMexico;
                case "NY":
                case "NEW YORK":
                    return State.NewYork;
                case "NC":
                case "NORTH CAROLINA":
                    return State.NorthCarolina;
                case "ND":
                case "NORTH DAKOTA":
                    return State.NorthDakota;
                case "OH":
                case "OHIO":
                    return State.Ohio;
                case "OK":
                case "OKLAHOMA":
                    return State.Oklahoma;
                case "OR":
                case "OREGON":
                    return State.Oregon;
                case "PA":
                case "PENNSYLVANIA":
                    return State.Pennsylvania;
                case "RI":
                case "RHODE ISLAND":
                    return State.RhodeIsland;
                case "SC":
                case "SOUTH CAROLINA":
                    return State.SouthCarolina;
                case "SD":
                case "SOUTH DAKOTA":
                    return State.SouthDakota;
                case "TN":
                case "TENNESSEE":
                    return State.Tennessee;
                case "TX":
                case "TEXAS":
                    return State.Texas;
                case "UT":
                case "UTAH":
                    return State.Utah;
                case "VT":
                case "VERMONT":
                    return State.Vermont;
                case "VA":
                case "VIRGINIA":
                    return State.Virginia;
                case "WA":
                case "WASHINGTON":
                    return State.Washington;
                case "WV":
                case "WEST VIRGINIA":
                    return State.WestVirginia;
                case "WI":
                case "WISCONSIN":
                    return State.Wisconsin;
                case "WY":
                case "WYOMING":
                    return State.Wyoming;
                default:
                    throw new Exception($"Unknown state requested '{stateString}'");
            }
        }

        /// <summary>
        /// A key pair value of the state and it's postal code.  The post code is the Key and the state name is the Value.
        /// </summary>
        public static Dictionary<string, string> StateList()
        {
            var lst = new Dictionary<string, string>
            {
                {"AL", "Alabama"},
                {"AK", "Alaska"},
                {"AZ", "Arizona"},
                {"AR", "Arkansas"},
                {"CA", "California"},
                {"CO", "Colorado"},
                {"CT", "Connecticut"},
                {"DE", "Delaware"},
                {"FL", "Florida"},
                {"GA", "Georgia"},
                {"HI", "Hawaii"},
                {"ID", "Idaho"},
                {"IL", "Illinois"},
                {"IN", "Indiana"},
                {"IA", "Iowa"},
                {"KS", "Kansas"},
                {"KY", "Kentucky"},
                {"LA", "Louisiana"},
                {"ME", "Maine"},
                {"MD", "Maryland"},
                {"MA", "Massachusetts"},
                {"MI", "Michigan"},
                {"MN", "Minnesota"},
                {"MS", "Mississippi"},
                {"MO", "Missouri"},
                {"MT", "Montana"},
                {"NE", "Nebraska"},
                {"NV", "Nevada"},
                {"NH", "New Hampshire"},
                {"NJ", "New Jersey"},
                {"NM", "New Mexico"},
                {"NY", "New York"},
                {"NC", "North Carolina"},
                {"ND", "North Dakota"},
                {"OH", "Ohio"},
                {"OK", "Oklahoma"},
                {"OR", "Oregon"},
                {"PA", "Pennsylvania"},
                {"RI", "Rhode Island"},
                {"SC", "South Carolina"},
                {"SD", "South Dakota"},
                {"TN", "Tennessee"},
                {"TX", "Texas"},
                {"UT", "Utah"},
                {"VT", "Vermont"},
                {"VA", "Virginia"},
                {"WA", "Washington"},
                {"WV", "West Virginia"},
                {"WI", "Wisconsin"},
                {"WY", "Wyoming"}
            };

            return lst;
        }
    }
}