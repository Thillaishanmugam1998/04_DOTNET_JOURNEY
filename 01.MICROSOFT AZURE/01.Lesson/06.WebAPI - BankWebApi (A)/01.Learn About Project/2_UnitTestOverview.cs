using System;

namespace Bank_Account_API._01.Learn_About_Project
{
    // ═══════════════════════════════════════════════════════════════
    //  UNIT TESTING — LEARNING NOTES
    //  Idhu oru "notes class" — code illa, concepts ah
    //  region ah organize panni vechirukom
    // ═══════════════════════════════════════════════════════════════
    public class _2_UnitTestOverview
    {
        #region 1_What_Is_Unit_Test

        /*
         * ─────────────────────────────────────────────
         *  WHAT IS UNIT TEST?
         * ─────────────────────────────────────────────
         *
         *  DEFINITION:
         *  "Unit test" na — namma code la irukura
         *  ORU ORU METHOD/CLASS ah THANIYA (isolated ah)
         *  test panradhu, real DB/network ILLAMA.
         *
         *  "UNIT" na enna?
         *  → Oru chinna testable piece of code
         *  → Example: Oru method — WithdrawAsync(), CalculateInterest()
         *  → Oru class — AccountService
         *
         *  KEY POINTS:
         *  ─────────────────────────────────────────────
         *  1. ISOLATED      → Real DB, API, file ILLAMA run aaganum
         *                     (Mock/Fake objects use panrom)
         *  2. FAST          → Milliseconds la run aagum
         *  3. REPEATABLE    → Eppovumum same result kudukanum
         *  4. AUTOMATIC     → "dotnet test" podhum — manual check venaam
         *  5. ONE unit      → Oru test = oru behaviour mattum check
         *
         *  SIMPLE EXAMPLE:
         * ─────────────────────────────────────────────
         *  BankService la WithdrawAsync() method irukku nu solalam.
         *
         *  Unit test = "Balance 5000 irukku, 2000 withdraw panna,
         *               result 3000 varudha?" nu check panradhu.
         *
         *  ⚠️ IMPORTANT: Real SQL Server connect PANNOM!
         *  → Mock repository use panrom (fake data return pannum)
         *
         *  WHY UNIT TEST?
         *  ─────────────────────────────────────────────
         *  ✅ Bug ah DEV time la catch — production customer ku teriyama
         *  ✅ Code change pannumbothu — tests pass = onnum odala nu confirm
         *  ✅ Manual testing (Postman 100 times) venaam — time save
         *  ✅ CI/CD pipeline la automatic run — broken code deploy aagave aagadhu
         */

        #endregion

        #region 2_Types_Of_Testing

        /*
         * ─────────────────────────────────────────────
         *  TYPES OF TESTING (TEST PYRAMID)
         * ─────────────────────────────────────────────
         *
         *              /  E2E  \           ← Slowest, kammi tests
         *             /---------\
         *            / Integration\        ← Medium speed
         *           /-------------\
         *          /  UNIT TESTS   \      ← Fastest, MAXIMUM tests inga dhan!
         *         /-----------------\
         *
         *  ─────────────────────────────────────────────
         *  TYPE 1: UNIT TEST
         *  ─────────────────────────────────────────────
         *  • ENNA?    Oru method/class ah thaniya test
         *  • REAL DB? ❌ Illa — Mock use panrom
         *  • SPEED?   ⚡ Milliseconds
         *  • EVLO?    Numerous — 100+ tests normal
         *  • EXAMPLE: AccountService.WithdrawAsync() — balance logic check
         *  • TOOL:    xUnit + Moq
         *
         *  ─────────────────────────────────────────────
         *  TYPE 2: INTEGRATION TEST
         *  ─────────────────────────────────────────────
         *  • ENNA?    Multiple layers serndhu work aagudha nu test
         *             (Controller + Routing + Middleware + Service + DB)
         *  • REAL DB? ✅ Aam — InMemory DB / Test DB use panrom
         *  • SPEED?   🐢 Seconds (app start aaganum)
         *  • EVLO?    Kammi — 10-20 key scenarios mattum
         *  • EXAMPLE: GET /api/banks/1 call panni → 200 OK + JSON varudha nu check
         *  • TOOL:    xUnit + WebApplicationFactory
         *
         *  ─────────────────────────────────────────────
         *  TYPE 3: E2E (END-TO-END) TEST
         *  ─────────────────────────────────────────────
         *  • ENNA?    Full system ah user maari test
         *             (UI → API → DB → response)
         *  • REAL DB? ✅ Real environment ellam
         *  • SPEED?   🐌 Slowest
         *  • EVLO?    Romba kammi — 2-5 critical flows mattum
         *  • EXAMPLE: Login panni → account create → withdraw → logout
         *  • TOOL:    Postman Collections, Playwright, Selenium
         *
         *  ─────────────────────────────────────────────
         *  NAMMA BANK API KU RULE:
         *  ─────────────────────────────────────────────
         *  SERVICE layer ku    → UNIT TEST         (business logic inga!)
         *  CONTROLLER/API ku   → INTEGRATION TEST  (routing + HTTP inga!)
         *
         *  REASON:
         *  → Service la LOGIC irukku       → isolated ah test panna mudiyum
         *  → Controller la WIRING irukku   → full app run pannala work aagadhu
         * */

        #endregion

        #region 3_Car_Analogy_Example

        /*
         * ─────────────────────────────────────────────
         *  🚗 CAR ANALOGY — TESTING AH PURIYA VACHIKALAM
         * ─────────────────────────────────────────────
         *
         *  Oru car ah oru SOFTWARE APP maari nenachu paaru!
         *
         *  Car la irukura parts:  Engine, Brake, Light, Gear, Horn, Battery
         *  App la irukura parts:  Service, Repository, Controller, Validator
         *
         * ─────────────────────────────────────────────
         *  SCENARIO 1: UNIT TEST = Oru Part ah Thaniya Check 🧪
         * ─────────────────────────────────────────────
         *
         *  Garage la mechanic:
         *    → Brake pedal ah thani ah press panni paakan
         *    → "Brake work aagudha?" — car road la POIKITTU test pannala!
         *    → Test bench la vechu, isolated ah check panraan
         *
         *  = UNIT TEST!
         *
         *  Code la:
         *    → WithdrawAsync() method ah thani ah test panrom
         *    → Real DB illama (Mock repo), "balance 5000 - 2000 = 3000 varudha?"
         *    → Method mattum — full app start pannala!
         *
         *  🚗 Brake test (garage bench)  =  🧪 Service unit test (Mock)
         *
         * ─────────────────────────────────────────────
         *  SCENARIO 2: INTEGRATION TEST = Parts Serndhu Work Aagudha? 🔗
         * ─────────────────────────────────────────────
         *
         *  Ippo mechanic:
         *    → Engine start pannu → Gear podu → Brake press pannu
         *    → Engine + Gear + Brake ELLAM serndhu sariya velai seiyudha?
         *    → Car la stand la vechu test (road la illa)
         *
         *  = INTEGRATION TEST!
         *
         *  Code la:
         *    → GET /api/accounts/1/withdraw request anuprom
         *    → Routing + Controller + Service + DB (InMemory) ELLAM real
         *    → "Full chain serndhu 200 OK response varudha?"
         *
         *  🚗 Engine+Gear+Brake serndhu (stand la)  =  🔗 Controller + Service + DB
         *
         * ─────────────────────────────────────────────
         *  SCENARIO 3: E2E TEST = Full Car ah Road La Ottu 🛣️
         * ─────────────────────────────────────────────
         *
         *  Test driver:
         *    → Car ah road la otturaan
         *    → Start → Drive → Brake → Turn → Park
         *    → FULL journey — real road, real traffic
         *    → "Customer experience eppadi irukum?"
         *
         *  = E2E TEST!
         *
         *  Code la:
         *    → Full deployed system — User interface → API → Real DB
         *    → Login panni → Money transfer panni → Receipt check panra flow
         *
         *  🚗 Road la full drive  =  🛣️ Full system flow test
         *
         * ─────────────────────────────────────────────
         *  SUMMARY TABLE:
         * ─────────────────────────────────────────────
         *
         *  ┌──────────────────┬──────────────────────┬─────────────────────────┐
         *  │ TEST TYPE        │ CAR ANALOGY          │ BANK API EXAMPLE        │
         *  ├──────────────────┼──────────────────────┼─────────────────────────┤
         *  │ Unit Test        │ Brake ah bench la    │ WithdrawAsync() logic   │
         *  │                  │ thani ah check       │ Mock repo oda test      │
         *  ├──────────────────┼──────────────────────┼─────────────────────────┤
         *  │ Integration Test │ Engine+Gear+Brake    │ GET /api/banks →        │
         *  │                  │ serndhu stand la     │ Routing+Controller+     │
         *  │                  │ test                 │ Service+InMemory DB     │
         *  ├──────────────────┼──────────────────────┼─────────────────────────┤
         *  │ E2E Test         │ Full car ah road la  │ Full user flow —        │
         *  │                  │ otti paakradhu       │ login → transfer → receipt │
         *  └──────────────────┴──────────────────────┴─────────────────────────┘
         *
         *  WHY PYRAMID SHAPE?
         *  → Unit test   = cheap + fast  → ATHIGAMA eluthanum (100+)
         *  → Integration = medium cost   → Key scenarios mattum (10-20)
         *  → E2E         = expensive     → Critical flows mattum (2-5)
         *
         *  💡 GOLDEN RULE:
         *  "Brake fault ah bench la (unit test) 1 minute la fix panna mudiyum.
         *   Aana road accident (production bug) aana — cost HUGE!
         *   Adhaan maximum tests UNIT level la vechukanum!"
         */

        #endregion
    }
}