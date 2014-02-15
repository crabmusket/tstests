//-----------------------------------------------------------------------------
// Tasman: A TorqueScript unit-testing library

new ScriptObject(Tasman) {
   suites = new SimSet();
   reporter = new ScriptObject(TasmanConsoleReporter);
};

function Tasman::runAll(%this) {
   %this.reporter.begin("_all");
   foreach(%suite in Tasman.suites) {
      %this.reporter.begin(%suite.subject);
      %this._currentSuite = %suite;

      %suite.passes = 0;
      %suite.fails = 0;
      %suite.count = 0;

      %tester = %suite.tester;
      %methods = %tester.dumpMethods();

      for(%i = 0; %i < %methods.count(); %i++) {
         %info = %methods.getValue(%i);
         %declaredIn = getRecord(%info, 3);
         if(%declaredIn !$= "" && -1 != strpos(%declaredIn, ".spec.cs")) {
            %suite.count++;
            // Run a test!
            if(%suite.isMethod("before")) {
               %suite.before();
            }
            %tester.call(%methods.getKey(%i));
            if(%suite.isMethod("after")) {
               %suite.after();
            }
         }
      }

      %this.reporter.reportSuite(%suite);
      %this.reporter.end();

      %methods.delete();
      %suite.passes = 0;
      %suite.fails = 0;
      %suite.count = 0;

      %this._currentSuite = "";
   }
   %this.reporter.end();
}

function Tasman::cleanUp(%this) {
   %this.suites.deleteAllObjects();
}

function Tasman::globals(%this, %on) {
   if(%on) {
      activatePackage(TasmanGlobals);
   } else {
      deactivatePackage(TasmanGlobals);
   }
}

package TasmanGlobals {
   function test(%name) { return Tasman.test(%name); }
   function expect(%value) { return Tasman.expect(%value); }
};

function Tasman::test(%this, %name) {
   new ScriptObject(%name @ Tests) {
      subject = %name;
      class = Suite;
      tester = new ScriptObject(%name @ Should) {
         class = SomethingShould;
      };
   };
}

function Suite::onAdd(%this) {
   Tasman.suites.add(%this);
}

function Suite::onRemove(%this) {
   %this.tester.delete();
}

function Tasman::expect(%this, %value) {
   return new ScriptObject() {
      class = Expectation;
      value = %value;
      inverted = false;
      suite = Tasman._currentSuite;
   };
}

function Expectation::not(%this) {
   %this.inverted = !%this.inverted;
   return %this;
}

function Expectation::toBeDefined(%this) {
   %this._test(%this.value !$= "");
   return %this;
}

function Expectation::toEqual(%this, %target) {
   %this._test(%this.value $= %target);
   return %this;
}

function Expectation::_test(%this, %pred) {
   %pass = %this.inverted ? !%pred : %pred;
   if(%pass) {
      %this.suite.passes++;
   } else {
      %this.suite.fails++;
   }
   return %this;
}

function TasmanConsoleReporter::begin(%this, %group) {
   if(%group !$= "_all") {
      echo("Testing" SPC %group @ "...");
   }
}

function TasmanConsoleReporter::end(%this) {
}

function TasmanConsoleReporter::reportSuite(%this, %suite) {
   if(%suite.fails > 0) {
      error("Failed" SPC %suite.fails SPC "of" SPC %suite.count);
   } else {
      echo("Passed" SPC %suite.passes SPC "of" SPC %suite.count);
   }
}
