# The Standard React + TypeScript + Vite — Files — Rules

## Single Responsibility

**tsr-files-001** [ERROR] A file must contain one primary responsibility. Files named `helpers.ts`, `utils.ts`, or `common.ts` indicate a violation.

## Naming by Architectural Role

**tsr-files-002** [ERROR] File names must describe the architectural role of their primary export using the following patterns:

| Role | Pattern |
|---|---|
| Broker | `{domain}{Kind}Broker.ts` |
| Broker interface | `i{domain}{Kind}Broker.ts` |
| Foundation service | `{domain}Service.ts` |
| Service interface | `i{domain}Service.ts` |
| View service | `{domain}ViewService.ts` |
| View service interface | `i{domain}ViewService.ts` |
| Page | `{Domain}Page.tsx` |
| Component | `{Domain}{Purpose}.tsx` |
| Hook | `use{Purpose}.ts` |
| Model | `{Domain}.ts` or `{Domain}{Purpose}.ts` |

## Extensions

**tsr-files-003** [ERROR] React component files must use the `.tsx` extension.

**tsr-files-004** [ERROR] Non-rendering TypeScript files (services, brokers, hooks, models, utilities) must use the `.ts` extension.

## Prohibited Patterns

**tsr-files-005** [ERROR] Generic file names (`utils`, `helpers`, `common`, `misc`, `shared`) are forbidden. Move code into a named broker, service, hook, model, or component.
