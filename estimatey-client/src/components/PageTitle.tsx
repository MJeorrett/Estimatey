export type PageTitleProps = {
    children: string | string[]
}

const PageTitle = ({ children }: PageTitleProps) => {
    return (
        <h1 className="text-5xl text-center mb-10">
            {children}
        </h1>
    )
}

export default PageTitle;
