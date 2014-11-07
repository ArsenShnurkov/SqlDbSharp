// =========================== LICENSE ===============================
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// ======================== EO LICENSE ===============================

using System;
using System.IO;

using org.rufwork;
using org.rufwork.mooresDb;
using org.rufwork.mooresDb.infrastructure;
using org.rufwork.mooresDb.infrastructure.commands;
using org.rufwork.mooresDb.infrastructure.contexts;
using org.rufwork.mooresDb.infrastructure.serializers;

public class MainClass
{
    // Right now, on Windows, that's "C:\\Users\\YourUserName\\Documents\\MooresDbPlay"
    //public static readonly string cstrDbDir = Utils.cstrHomeDir + Path.DirectorySeparatorChar + "MooresDbPlay";
    public static bool bDebug = false;
    public static string buildData = "20140906";    // not always incremented with each build.
    public static string version = "0.0.4.1";
}
