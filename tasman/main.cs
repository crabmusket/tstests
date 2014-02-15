//-----------------------------------------------------------------------------
// Tasman: A TorqueScript unit-testing library

new ScriptObject(Tasman) {
   suites = new SimSet();
   reporter = new ScriptObject(TasmanConsoleReporter);
};

function Tasman::runAll(%this) {
   %this.reporter.begin("_all");
   %dummy = new ScriptObject() { class = SomethingShould; };

   foreach(%suite in Tasman.suites) {
      %this.reporter.begin(%suite.subject);
      %this._currentSuite = %suite;

      %suite.passes = 0;
      %suite.fails = 0;
      %suite.count = 0;

      %tester = %suite.tester;
      %methods = %tester.dumpMethods();

      for(%i = 0; %i < %methods.count(); %i++) {
         %method = %methods.getKey(%i);

         // We only want methods unique to this tester, so we compare against an
         // instance of the SomethingShould base class.
         if(!%dummy.isMethod(%method)) {
            %suite._currentMethod = %method;
            %suite.count++;

            // Run the test!
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
      %suite._currentMethod = "";

      %this._currentSuite = "";
   }

   %dummy.delete();
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

function Expectation::toBeAnObject(%this) {
   %this._test(isObject(%this.value), "expected" SPC %this.value SPC "to be an object");
   return %this;
}

function Expectation::toBeDefined(%this) {
   %this._test(%this.value !$= "", "expected a non-empty string");
   return %this;
}

function Expectation::toEqual(%this, %target) {
   %this._test(%this.value $= %target, "expected" SPC %this.value SPC "to equal" SPC %target);
   return %this;
}

function Expectation::_test(%this, %pred, %failMessage) {
   %pass = %this.inverted ? !%pred : %pred;
   if(%pass) {
      %this.suite.passes++;
   } else {
      %this.suite.fails++;
      %niceMethod = strreplace(%this.suite._currentMethod, "_", " ");
      error(%this.suite.subject SPC "should" SPC %niceMethod @ ":" SPC %failMessage);
   }
   return %this;
}

function TasmanConsoleReporter::begin(%this, %group) {
   if(%group $= "_all") {
      %this.depth = 0;
      echo("==================================================");
      echo("Running Tasman test suite!");
      echo("");
   } else {
      echo("Testing" SPC %group @ "...");
   }
   %this.depth++;
}

function TasmanConsoleReporter::end(%this) {
   %this.depth--;
   if(0 == %this.depth) {
      echo("That concludes this Tasman test session");
      echo("==================================================");
   }
}

function TasmanConsoleReporter::reportSuite(%this, %suite) {
   if(%suite.fails > 0) {
      error(%suite.subject SPC "failed" SPC %suite.fails SPC "of" SPC %suite.count);
   } else {
      echo(%suite.subject SPC "passed" SPC %suite.passes SPC "of" SPC %suite.count);
   }
   echo("");
}
