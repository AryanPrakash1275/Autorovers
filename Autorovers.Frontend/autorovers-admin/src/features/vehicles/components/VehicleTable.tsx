import type { Vehicle } from "../types";

interface VehicleTableProps {
  vehicles: Vehicle[];
  sortBy: string;
  sortDir: "asc" | "desc";
  onSortChange: (key: string) => void;
  onEdit: (vehicle: Vehicle) => void;
  onDelete: (id: number) => void;
}

const Th: React.FC<
  React.PropsWithChildren<{
    sortKey?: string;
    activeSortKey: string;
    sortDir: "asc" | "desc";
    onSortChange?: (key: string) => void;
  }>
> = ({ children, sortKey, activeSortKey, sortDir, onSortChange }) => {
  const isActive = sortKey && sortKey === activeSortKey;

  const handleClick = () => {
    if (sortKey && onSortChange) onSortChange(sortKey);
  };

  return (
    <th
      onClick={handleClick}
      className="table-th"
      style={{ cursor: sortKey ? "pointer" : "default" }}
    >
      {children}
      {isActive && (
        <span className="sort-indicator">
          {sortDir === "asc" ? "▲" : "▼"}
        </span>
      )}
    </th>
  );
};

const Td: React.FC<React.PropsWithChildren<{ className?: string }>> = ({
  children,
  className,
}) => <td className={`table-td ${className ?? ""}`}>{children}</td>;

export function VehicleTable({
  vehicles,
  sortBy,
  sortDir,
  onSortChange,
  onEdit,
  onDelete,
}: VehicleTableProps) {
  return (
    <div className="admin-table-wrapper">
      <table className="admin-table">
        <thead>
          <tr>
            <Th activeSortKey={sortBy} sortDir={sortDir}>
              Id
            </Th>
            <Th sortKey="brand" activeSortKey={sortBy} sortDir={sortDir} onSortChange={onSortChange}>
              Brand
            </Th>
            <Th sortKey="model" activeSortKey={sortBy} sortDir={sortDir} onSortChange={onSortChange}>
              Model
            </Th>
            <Th sortKey="variant" activeSortKey={sortBy} sortDir={sortDir} onSortChange={onSortChange}>
              Variant
            </Th>
            <Th sortKey="year" activeSortKey={sortBy} sortDir={sortDir} onSortChange={onSortChange}>
              Year
            </Th>
            <Th sortKey="price" activeSortKey={sortBy} sortDir={sortDir} onSortChange={onSortChange}>
              Price
            </Th>
            <Th sortKey="category" activeSortKey={sortBy} sortDir={sortDir} onSortChange={onSortChange}>
              Category
            </Th>
            <Th sortKey="transmission" activeSortKey={sortBy} sortDir={sortDir} onSortChange={onSortChange}>
              Transmission
            </Th>
            <Th sortKey="slug" activeSortKey={sortBy} sortDir={sortDir} onSortChange={onSortChange}>
              Slug
            </Th>
            <Th activeSortKey={sortBy} sortDir={sortDir}>
              Actions
            </Th>
          </tr>
        </thead>

        <tbody>
          {vehicles.map((v) => (
            <tr key={v.id}>
              <Td>{v.id}</Td>
              <Td>{v.brand}</Td>
              <Td>{v.model}</Td>
              <Td>{v.variant}</Td>
              <Td>{v.year}</Td>
              <Td>₹{(v.price ?? 0).toLocaleString()}</Td>
              <Td>{v.category}</Td>
              <Td>{v.transmission}</Td>
              <Td className="break-anywhere">{v.slug}</Td>
              <Td>
                <button className="btn btn-sm" onClick={() => onEdit(v)}>
                  Edit
                </button>
                <button
                  className="btn btn-sm btn-danger"
                  onClick={() => onDelete(v.id)}
                  style={{ marginLeft: "0.4rem" }}
                >
                  Delete
                </button>
              </Td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
