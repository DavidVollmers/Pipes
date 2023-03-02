import AppSideBar from './app-side-bar'

export interface AppProps {
  readonly title: JSX.Element
  readonly children: JSX.Element
}

export default function App({ title, children }: AppProps) {
  return (
    <>
      <AppSideBar></AppSideBar>
      <div className="flex flex-1 flex-col lg:pl-64">
        <main className="flex-1">
          <div className="py-6">
            <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">{title}</div>
            <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">{children}</div>
          </div>
        </main>
      </div>
    </>
  )
}
