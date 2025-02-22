﻿namespace CodinaxProjectMvc.DataList
{

    public sealed class Country
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Phone { get; set; }
    }

    public static class Countries
    {
        public static List<Country> All = new List<Country>()
        {
            new Country() { Name = "Afghanistan", Code = "AF", Phone = 93 },
            new Country() { Name = "Aland Islands", Code = "AX", Phone = 358 },
            new Country() { Name = "Albania", Code = "AL", Phone = 355 },
            new Country() { Name = "Algeria", Code = "DZ", Phone = 213 },
            new Country() { Name = "American Samoa", Code = "AS", Phone = 1684 },
            new Country() { Name = "Andorra", Code = "AD", Phone = 376 },
            new Country() { Name = "Angola", Code = "AO", Phone = 244 },
            new Country() { Name = "Anguilla", Code = "AI", Phone = 1264 },
            new Country() { Name = "Antarctica", Code = "AQ", Phone = 672 },
            new Country() { Name = "Antigua and Barbuda", Code = "AG", Phone = 1268 },
            new Country() { Name = "Argentina", Code = "AR", Phone = 54 },
            new Country() { Name = "Armenia", Code = "AM", Phone = 374 },
            new Country() { Name = "Aruba", Code = "AW", Phone = 297 },
            new Country() { Name = "Australia", Code = "AU", Phone = 61 },
            new Country() { Name = "Austria", Code = "AT", Phone = 43 },
            new Country() { Name = "Azerbaijan", Code = "AZ", Phone = 994 },
            new Country() { Name = "Bahamas", Code = "BS", Phone = 1242 },
            new Country() { Name = "Bahrain", Code = "BH", Phone = 973 },
            new Country() { Name = "Bangladesh", Code = "BD", Phone = 880 },
            new Country() { Name = "Barbados", Code = "BB", Phone = 1246 },
            new Country() { Name = "Belarus", Code = "BY", Phone = 375 },
            new Country() { Name = "Belgium", Code = "BE", Phone = 32 },
            new Country() { Name = "Belize", Code = "BZ", Phone = 501 },
            new Country() { Name = "Benin", Code = "BJ", Phone = 229 },
            new Country() { Name = "Bermuda", Code = "BM", Phone = 1441 },
            new Country() { Name = "Bhutan", Code = "BT", Phone = 975 },
            new Country() { Name = "Bolivia", Code = "BO", Phone = 591 },
            new Country() { Name = "Bonaire, Sint Eustatius and Saba", Code = "BQ", Phone = 599 },
            new Country() { Name = "Bosnia and Herzegovina", Code = "BA", Phone = 387 },
            new Country() { Name = "Botswana", Code = "BW", Phone = 267 },
            new Country() { Name = "Bouvet Island", Code = "BV", Phone = 55 },
            new Country() { Name = "Brazil", Code = "BR", Phone = 55 },
            new Country() { Name = "British Indian Ocean Territory", Code = "IO", Phone = 246 },
            new Country() { Name = "Brunei Darussalam", Code = "BN", Phone = 673 },
            new Country() { Name = "Bulgaria", Code = "BG", Phone = 359 },
            new Country() { Name = "Burkina Faso", Code = "BF", Phone = 226 },
            new Country() { Name = "Burundi", Code = "BI", Phone = 257 },
            new Country() { Name = "Cambodia", Code = "KH", Phone = 855 },
            new Country() { Name = "Cameroon", Code = "CM", Phone = 237 },
            new Country() { Name = "Canada", Code = "CA", Phone = 1 },
            new Country() { Name = "Cape Verde", Code = "CV", Phone = 238 },
            new Country() { Name = "Cayman Islands", Code = "KY", Phone = 1345 },
            new Country() { Name = "Central African Republic", Code = "CF", Phone = 236 },
            new Country() { Name = "Chad", Code = "TD", Phone = 235 },
            new Country() { Name = "Chile", Code = "CL", Phone = 56 },
            new Country() { Name = "China", Code = "CN", Phone = 86 },
            new Country() { Name = "Christmas Island", Code = "CX", Phone = 61 },
            new Country() { Name = "Cocos (Keeling) Islands", Code = "CC", Phone = 672 },
            new Country() { Name = "Colombia", Code = "CO", Phone = 57 },
            new Country() { Name = "Comoros", Code = "KM", Phone = 269 },
            new Country() { Name = "Congo", Code = "CG", Phone = 242 },
            new Country() { Name = "Congo, Democratic Republic of the Congo", Code = "CD", Phone = 242 },
            new Country() { Name = "Cook Islands", Code = "CK", Phone = 682 },
            new Country() { Name = "Costa Rica", Code = "CR", Phone = 506 },
            new Country() { Name = "Cote D'Ivoire", Code = "CI", Phone = 225 },
            new Country() { Name = "Croatia", Code = "HR", Phone = 385 },
            new Country() { Name = "Cuba", Code = "CU", Phone = 53 },
            new Country() { Name = "Curacao", Code = "CW", Phone = 599 },
            new Country() { Name = "Cyprus", Code = "CY", Phone = 357 },
            new Country() { Name = "Czech Republic", Code = "CZ", Phone = 420 },
            new Country() { Name = "Denmark", Code = "DK", Phone = 45 },
            new Country() { Name = "Djibouti", Code = "DJ", Phone = 253 },
            new Country() { Name = "Dominica", Code = "DM", Phone = 1767 },
            new Country() { Name = "Dominican Republic", Code = "DO", Phone = 1809 },
            new Country() { Name = "Ecuador", Code = "EC", Phone = 593 },
            new Country() { Name = "Egypt", Code = "EG", Phone = 20 },
            new Country() { Name = "El Salvador", Code = "SV", Phone = 503 },
            new Country() { Name = "Equatorial Guinea", Code = "GQ", Phone = 240 },
            new Country() { Name = "Eritrea", Code = "ER", Phone = 291 },
            new Country() { Name = "Estonia", Code = "EE", Phone = 372 },
            new Country() { Name = "Ethiopia", Code = "ET", Phone = 251 },
            new Country() { Name = "Falkland Islands (Malvinas)", Code = "FK", Phone = 500 },
            new Country() { Name = "Faroe Islands", Code = "FO", Phone = 298 },
            new Country() { Name = "Fiji", Code = "FJ", Phone = 679 },
            new Country() { Name = "Finland", Code = "FI", Phone = 358 },
            new Country() { Name = "France", Code = "FR", Phone = 33 },
            new Country() { Name = "French Guiana", Code = "GF", Phone = 594 },
            new Country() { Name = "French Polynesia", Code = "PF", Phone = 689 },
            new Country() { Name = "French Southern Territories", Code = "TF", Phone = 262 },
            new Country() { Name = "Gabon", Code = "GA", Phone = 241 },
            new Country() { Name = "Gambia", Code = "GM", Phone = 220 },
            new Country() { Name = "Georgia", Code = "GE", Phone = 995 },
            new Country() { Name = "Germany", Code = "DE", Phone = 49 },
            new Country() { Name = "Ghana", Code = "GH", Phone = 233 },
            new Country() { Name = "Gibraltar", Code = "GI", Phone = 350 },
            new Country() { Name = "Greece", Code = "GR", Phone = 30 },
            new Country() { Name = "Greenland", Code = "GL", Phone = 299 },
            new Country() { Name = "Grenada", Code = "GD", Phone = 1473 },
            new Country() { Name = "Guadeloupe", Code = "GP", Phone = 590 },
            new Country() { Name = "Guam", Code = "GU", Phone = 1671 },
            new Country() { Name = "Guatemala", Code = "GT", Phone = 502 },
            new Country() { Name = "Guernsey", Code = "GG", Phone = 44 },
            new Country() { Name = "Guinea", Code = "GN", Phone = 224 },
            new Country() { Name = "Guinea-Bissau", Code = "GW", Phone = 245 },
            new Country() { Name = "Guyana", Code = "GY", Phone = 592 },
            new Country() { Name = "Haiti", Code = "HT", Phone = 509 },
            new Country() { Name = "Heard Island and McDonald Islands", Code = "HM", Phone = 0 },
            new Country() { Name = "Holy See (Vatican City State)", Code = "VA", Phone = 39 },
            new Country() { Name = "Honduras", Code = "HN", Phone = 504 },
            new Country() { Name = "Hong Kong", Code = "HK", Phone = 852 },
            new Country() { Name = "Hungary", Code = "HU", Phone = 36 },
            new Country() { Name = "Iceland", Code = "IS", Phone = 354 },
            new Country() { Name = "India", Code = "IN", Phone = 91 },
            new Country() { Name = "Indonesia", Code = "ID", Phone = 62 },
            new Country() { Name = "Iran, Islamic Republic of", Code = "IR", Phone = 98 },
            new Country() { Name = "Iraq", Code = "IQ", Phone = 964 },
            new Country() { Name = "Ireland", Code = "IE", Phone = 353 },
            new Country() { Name = "Isle of Man", Code = "IM", Phone = 44 },
            new Country() { Name = "Israel", Code = "IL", Phone = 972 },
            new Country() { Name = "Italy", Code = "IT", Phone = 39 },
            new Country() { Name = "Jamaica", Code = "JM", Phone = 1876 },
            new Country() { Name = "Japan", Code = "JP", Phone = 81 },
            new Country() { Name = "Jersey", Code = "JE", Phone = 44 },
            new Country() { Name = "Jordan", Code = "JO", Phone = 962 },
            new Country() { Name = "Kazakhstan", Code = "KZ", Phone = 7 },
            new Country() { Name = "Kenya", Code = "KE", Phone = 254 },
            new Country() { Name = "Kiribati", Code = "KI", Phone = 686 },
            new Country() { Name = "Korea, Democratic People's Republic of", Code = "KP", Phone = 850 },
            new Country() { Name = "Korea, Republic of", Code = "KR", Phone = 82 },
            new Country() { Name = "Kosovo", Code = "XK", Phone = 383 },
            new Country() { Name = "Kuwait", Code = "KW", Phone = 965 },
            new Country() { Name = "Kyrgyzstan", Code = "KG", Phone = 996 },
            new Country() { Name = "Lao People's Democratic Republic", Code = "LA", Phone = 856 },
            new Country() { Name = "Latvia", Code = "LV", Phone = 371 },
            new Country() { Name = "Lebanon", Code = "LB", Phone = 961 },
            new Country() { Name = "Lesotho", Code = "LS", Phone = 266 },
            new Country() { Name = "Liberia", Code = "LR", Phone = 231 },
            new Country() { Name = "Libyan Arab Jamahiriya", Code = "LY", Phone = 218 },
            new Country() { Name = "Liechtenstein", Code = "LI", Phone = 423 },
            new Country() { Name = "Lithuania", Code = "LT", Phone = 370 },
            new Country() { Name = "Luxembourg", Code = "LU", Phone = 352 },
            new Country() { Name = "Macao", Code = "MO", Phone = 853 },
            new Country() { Name = "Macedonia, the Former Yugoslav Republic of", Code = "MK", Phone = 389 },
            new Country() { Name = "Madagascar", Code = "MG", Phone = 261 },
            new Country() { Name = "Malawi", Code = "MW", Phone = 265 },
            new Country() { Name = "Malaysia", Code = "MY", Phone = 60 },
            new Country() { Name = "Maldives", Code = "MV", Phone = 960 },
            new Country() { Name = "Mali", Code = "ML", Phone = 223 },
            new Country() { Name = "Malta", Code = "MT", Phone = 356 },
            new Country() { Name = "Marshall Islands", Code = "MH", Phone = 692 },
            new Country() { Name = "Martinique", Code = "MQ", Phone = 596 },
            new Country() { Name = "Mauritania", Code = "MR", Phone = 222 },
            new Country() { Name = "Mauritius", Code = "MU", Phone = 230 },
            new Country() { Name = "Mayotte", Code = "YT", Phone = 262 },
            new Country() { Name = "Mexico", Code = "MX", Phone = 52 },
            new Country() { Name = "Micronesia, Federated States of", Code = "FM", Phone = 691 },
            new Country() { Name = "Moldova, Republic of", Code = "MD", Phone = 373 },
            new Country() { Name = "Monaco", Code = "MC", Phone = 377 },
            new Country() { Name = "Mongolia", Code = "MN", Phone = 976 },
            new Country() { Name = "Montenegro", Code = "ME", Phone = 382 },
            new Country() { Name = "Montserrat", Code = "MS", Phone = 1664 },
            new Country() { Name = "Morocco", Code = "MA", Phone = 212 },
            new Country() { Name = "Mozambique", Code = "MZ", Phone = 258 },
            new Country() { Name = "Myanmar", Code = "MM", Phone = 95 },
            new Country() { Name = "Namibia", Code = "NA", Phone = 264 },
            new Country() { Name = "Nauru", Code = "NR", Phone = 674 },
            new Country() { Name = "Nepal", Code = "NP", Phone = 977 },
            new Country() { Name = "Netherlands", Code = "NL", Phone = 31 },
            new Country() { Name = "Netherlands Antilles", Code = "AN", Phone = 599 },
            new Country() { Name = "New Caledonia", Code = "NC", Phone = 687 },
            new Country() { Name = "New Zealand", Code = "NZ", Phone = 64 },
            new Country() { Name = "Nicaragua", Code = "NI", Phone = 505 },
            new Country() { Name = "Niger", Code = "NE", Phone = 227 },
            new Country() { Name = "Nigeria", Code = "NG", Phone = 234 },
            new Country() { Name = "Niue", Code = "NU", Phone = 683 },
            new Country() { Name = "Norfolk Island", Code = "NF", Phone = 672 },
            new Country() { Name = "Northern Mariana Islands", Code = "MP", Phone = 1670 },
            new Country() { Name = "Norway", Code = "NO", Phone = 47 },
            new Country() { Name = "Oman", Code = "OM", Phone = 968 },
            new Country() { Name = "Pakistan", Code = "PK", Phone = 92 },
            new Country() { Name = "Palau", Code = "PW", Phone = 680 },
            new Country() { Name = "Palestinian Territory, Occupied", Code = "PS", Phone = 970 },
            new Country() { Name = "Panama", Code = "PA", Phone = 507 },
            new Country() { Name = "Papua New Guinea", Code = "PG", Phone = 675 },
            new Country() { Name = "Paraguay", Code = "PY", Phone = 595 },
            new Country() { Name = "Peru", Code = "PE", Phone = 51 },
            new Country() { Name = "Philippines", Code = "PH", Phone = 63 },
            new Country() { Name = "Pitcairn", Code = "PN", Phone = 64 },
            new Country() { Name = "Poland", Code = "PL", Phone = 48 },
            new Country() { Name = "Portugal", Code = "PT", Phone = 351 },
            new Country() { Name = "Puerto Rico", Code = "PR", Phone = 1787 },
            new Country() { Name = "Qatar", Code = "QA", Phone = 974 },
            new Country() { Name = "Reunion", Code = "RE", Phone = 262 },
            new Country() { Name = "Romania", Code = "RO", Phone = 40 },
            new Country() { Name = "Russian Federation", Code = "RU", Phone = 7 },
            new Country() { Name = "Rwanda", Code = "RW", Phone = 250 },
            new Country() { Name = "Saint Barthelemy", Code = "BL", Phone = 590 },
            new Country() { Name = "Saint Helena", Code = "SH", Phone = 290 },
            new Country() { Name = "Saint Kitts and Nevis", Code = "KN", Phone = 1869 },
            new Country() { Name = "Saint Lucia", Code = "LC", Phone = 1758 },
            new Country() { Name = "Saint Martin", Code = "MF", Phone = 590 },
            new Country() { Name = "Saint Pierre and Miquelon", Code = "PM", Phone = 508 },
            new Country() { Name = "Saint Vincent and the Grenadines", Code = "VC", Phone = 1784 },
            new Country() { Name = "Samoa", Code = "WS", Phone = 684 },
            new Country() { Name = "San Marino", Code = "SM", Phone = 378 },
            new Country() { Name = "Sao Tome and Principe", Code = "ST", Phone = 239 },
            new Country() { Name = "Saudi Arabia", Code = "SA", Phone = 966 },
            new Country() { Name = "Senegal", Code = "SN", Phone = 221 },
            new Country() { Name = "Serbia", Code = "RS", Phone = 381 },
            new Country() { Name = "Serbia and Montenegro", Code = "CS", Phone = 381 },
            new Country() { Name = "Seychelles", Code = "SC", Phone = 248 },
            new Country() { Name = "Sierra Leone", Code = "SL", Phone = 232 },
            new Country() { Name = "Singapore", Code = "SG", Phone = 65 },
            new Country() { Name = "St Martin", Code = "SX", Phone = 721 },
            new Country() { Name = "Slovakia", Code = "SK", Phone = 421 },
            new Country() { Name = "Slovenia", Code = "SI", Phone = 386 },
            new Country() { Name = "Solomon Islands", Code = "SB", Phone = 677 },
            new Country() { Name = "Somalia", Code = "SO", Phone = 252 },
            new Country() { Name = "South Africa", Code = "ZA", Phone = 27 },
            new Country() { Name = "South Georgia and the South Sandwich Islands", Code = "GS", Phone = 500 },
            new Country() { Name = "South Sudan", Code = "SS", Phone = 211 },
            new Country() { Name = "Spain", Code = "ES", Phone = 34 },
            new Country() { Name = "Sri Lanka", Code = "LK", Phone = 94 },
            new Country() { Name = "Sudan", Code = "SD", Phone = 249 },
            new Country() { Name = "Suriname", Code = "SR", Phone = 597 },
            new Country() { Name = "Svalbard and Jan Mayen", Code = "SJ", Phone = 47 },
            new Country() { Name = "Swaziland", Code = "SZ", Phone = 268 },
            new Country() { Name = "Sweden", Code = "SE", Phone = 46 },
            new Country() { Name = "Switzerland", Code = "CH", Phone = 41 },
            new Country() { Name = "Syrian Arab Republic", Code = "SY", Phone = 963 },
            new Country() { Name = "Taiwan", Code = "TW", Phone = 886 },
            new Country() { Name = "Tajikistan", Code = "TJ", Phone = 992 },
            new Country() { Name = "Tanzania, United Republic of", Code = "TZ", Phone = 255 },
            new Country() { Name = "Thailand", Code = "TH", Phone = 66 },
            new Country() { Name = "Timor-Leste", Code = "TL", Phone = 670 },
            new Country() { Name = "Togo", Code = "TG", Phone = 228 },
            new Country() { Name = "Tokelau", Code = "TK", Phone = 690 },
            new Country() { Name = "Tonga", Code = "TO", Phone = 676 },
            new Country() { Name = "Trinidad and Tobago", Code = "TT", Phone = 1868 },
            new Country() { Name = "Tunisia", Code = "TN", Phone = 216 },
            new Country() { Name = "Turkey", Code = "TR", Phone = 90 },
            new Country() { Name = "Turkmenistan", Code = "TM", Phone = 993 },
            new Country() { Name = "Turks and Caicos Islands", Code = "TC", Phone = 1649 },
            new Country() { Name = "Tuvalu", Code = "TV", Phone = 688 },
            new Country() { Name = "Uganda", Code = "UG", Phone = 256 },
            new Country() { Name = "Ukraine", Code = "UA", Phone = 380 },
            new Country() { Name = "United Arab Emirates", Code = "AE", Phone = 971 },
            new Country() { Name = "United Kingdom", Code = "GB", Phone = 44 },
            new Country() { Name = "United States", Code = "US", Phone = 1 },
            new Country() { Name = "United States Minor Outlying Islands", Code = "UM", Phone = 1 },
            new Country() { Name = "Uruguay", Code = "UY", Phone = 598 },
            new Country() { Name = "Uzbekistan", Code = "UZ", Phone = 998 },
            new Country() { Name = "Vanuatu", Code = "VU", Phone = 678 },
            new Country() { Name = "Venezuela", Code = "VE", Phone = 58 },
            new Country() { Name = "Vietnam", Code = "VN", Phone = 84 },
            new Country() { Name = "Virgin Islands, British", Code = "VG", Phone = 1284 },
            new Country() { Name = "Virgin Islands, U.S.", Code = "VI", Phone = 1340 },
            new Country() { Name = "Wallis and Futuna", Code = "WF", Phone = 681 },
            new Country() { Name = "Western Sahara", Code = "EH", Phone = 212 },
            new Country() { Name = "Yemen", Code = "YE", Phone = 967 },
            new Country() { Name = "Zambia", Code = "ZM", Phone = 260 },
            new Country() { Name = "Zimbabwe", Code = "ZW", Phone = 263 }
        };

    }


}
