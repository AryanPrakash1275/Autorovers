type Props = {
  title: string;
  children: React.ReactNode;
};

export function AdminLayout({ title, children }: Props) {
  return (
    <div className="admin-root">
      <header className="admin-header">
        <h1>{title}</h1>
      </header>
      <main className="admin-main">{children}</main>
    </div>
  );
}
