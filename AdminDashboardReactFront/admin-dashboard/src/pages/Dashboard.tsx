import React, { useState } from "react";
import { useAdmin, User } from "../context/AdminContext";

const Dashboard: React.FC = () => {
  const { users, updateUser } = useAdmin();
  const [editingUserId, setEditingUserId] = useState<number | null>(null);
  const [editData, setEditData] = useState<Partial<User>>({});

  const startEdit = (user: User) => {
    setEditingUserId(user.id);
    setEditData({ ...user });
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setEditData((prev) => ({ ...prev, [name]: value }));
  };

  const handleBalanceChange = (token: string, value: string) => {
    const num = Number(value);
    if (!editData.balance) editData.balance = {};
    setEditData((prev) => ({
      ...prev,
      balance: {
        ...prev?.balance,
        [token]: isNaN(num) ? 0 : num,
      },
    }));
  };

  const saveChanges = () => {
    if (!editData.id) return;
    updateUser(editData as User);
    setEditingUserId(null);
  };

  const cancelEdit = () => setEditingUserId(null);

  return (
    <div style={{ padding: "2rem" }}>
      <h2>Админ-панель: Пользователи</h2>
      <table style={{ width: "100%", borderCollapse: "collapse" }}>
        <thead>
          <tr>
            <th>Email</th>
            <th>Имя</th>
            <th>Баланс токенов</th>
            <th>Действие</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id}>
              <td>
                {editingUserId === user.id ? (
                  <input
                    name="login"
                    value={editData.login || ""}
                    onChange={handleChange}
                  />
                ) : (
                  user.login
                )}
              </td>
              <td>
                {editingUserId === user.id ? (
                  <input
                    name="name"
                    value={(editData as any).name || ""}
                    onChange={handleChange}
                  />
                ) : (
                  (user as any).name || "-"
                )}
              </td>
              <td>
                {editingUserId === user.id ? (
                  <div>
                    {Object.entries(user.balance).map(([token, amount]) => (
                      <div key={token}>
                        <label>{token}: </label>
                        <input
                          type="number"
                          value={editData.balance?.[token] || 0}
                          onChange={(e) => handleBalanceChange(token, e.target.value)}
                        />
                      </div>
                    ))}
                  </div>
                ) : (
                  Object.entries(user.balance)
                    .map(([token, amount]) => `${token}: ${amount}`)
                    .join(", ")
                )}
              </td>
              <td>
                {editingUserId === user.id ? (
                  <>
                    <button onClick={saveChanges}>Сохранить</button>
                    <button onClick={cancelEdit}>Отмена</button>
                  </>
                ) : (
                  <button onClick={() => startEdit(user)}>Редактировать</button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Dashboard;
