[<< Back to README.md](./../README.md)

# Frontend
The frontend project, developed with ReactJS.
- Serves as the user interface for allowing visualization, control and monitoring for solutions being built.
- Allows for making API calls to the [backend](./backend.md) project.
- Allows for configuring the system visually.
- The projects static content is served by the [backend](./backend.md) project.

## Getting Started
- From inside of the frontend project root, `src/frontend`, open a terminal.
- Install all dependencies by running `npm i`.
- In order to start up a development server for the frontend, run `npm run build && npm run dev`.
  - This will spin up a dev server that you may use to access the static frontend content, however this step is tyically merely to enable auto-recompilation of client-side content, while the backend is running. This is because the backend hosts the frontend content via static file hosting. Since the backend is required for auto and the various API endpoints required by the frontend, one would rarely, if at all, spin up the frontend on it's own.

## Project Structure
- `public` - where static content goes that should be available in the dist dir.
  - `icons` - where various icons may be hosted, accessibel from `/icons/...` via HTML.
  - `appsettings.json` - where frontend app config lives which allows us to interpolate variables during the DevOps pipelines. NOTE: This JSON file should adhere to the [IState signature](./../src/frontend/src/interfaces/state/IState.ts).
- `src` - where the root of the project source code, beyond the app skeleton lives.
  - `components` - where all ReactJS components are housed. Use logical subdirectories here to group components.
  - `enums` - where TypeScript enumerations are housed.
    - `StateReducerActionTypes.ts` - all supported augmentation types of the global store.
  - `hooks` - where ReactJS hooks live. These are specialized services.
  - `interfaces` - where TypeScript interfaces are housed.
    - `state` - where state store interfaces are housed.
      - `IState.ts` - the schema of the global state store.
      - `IStateReducerAction.ts` - the interface for a request to augment the global state.
  - `reducers` - where state manipulation functions are housed.
    - `StateReducer.ts` - augment the global state based on the provided action.
  - `services` - where service components live. Typically vanilla TypeScript functional components. For this we adhere to the iDesign architectural principals. See [README.md](./../README.md) for more on that.
    - `data` - where data accessor components are housed.
    - `engines` - where engine components are housed.
    - `managers` - where manager components are housed.
  - `reducers` - where state manipulation functions are housed.
  - `App.css` - where the root-level CSS is housed for the container app component. Shared styling should go here.
  - `App.tsx` - where the root-level app component is housed. Shared functionality like views and navigation should go here.
  - `config.ts` - where the default config for the state store is housed. Ideally as much of this should be loaded by the appsettings.json file instead of being hard-coded here.
  - `main.tsx` - where the root-level component is housed. Shared functionality like bootstrapping the state and routing, goes here.
  - `vite-env.d.ts` - where typeScript imports are housed for Vite.
- `index.html` - the root entry HTML file for the frontend application.
- `package*.json` - where NPM package config lives.
- `tsconfig*.json` - where TypeScript compilation config lives.
- `vite.config.ts` - where the Vite template generator config lives.

# Architecture
## Managers
## Engines
## Data Access

## References
- [ReactJS Icons Explorer (Package already installed)](https://react-icons.github.io/react-icons/)