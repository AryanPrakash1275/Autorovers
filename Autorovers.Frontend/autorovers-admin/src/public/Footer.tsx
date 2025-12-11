export function Footer() {
  return (
    <footer
      style={{
        position: "fixed",
        bottom: 0,
        left: 0,
        width: "100%",
        padding: "0.75rem 0",
        textAlign: "center",
        color: "#888",
        fontSize: "0.85rem",
        background: "#f8f8f8",
        borderTop: "1px solid #e5e5e5",
        zIndex: 20,
      }}
    >
      © {new Date().getFullYear()} Autorovers — All rights reserved.
    </footer>
  );
}
