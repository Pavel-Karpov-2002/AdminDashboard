import React, { useState } from "react";
import { useAdmin, User } from "../context/AdminContext";

const UsersList: React.FC = () => {
  const { users, updateUser } = useAdmin();

  // Для редактирования — хранить ID редактируемого пользователя и данные формы
  const [editingUserId, setEditingUserId] = useState<number | null>(null);
  const [editData, setEditData] = useState<{ login: string; balance: Record<string, number> }>({
    login: "",
    balance: {},
  });

  // Начать редактирование
  const startEdit = (user: User) => {
    setEditingUserId(user.id);
    setEditData({ login: user.login, balance: { ...user.balance } });
  };

  // Обработка изменений полей login
  const onLoginChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEditData((prev) => ({ ...prev, login: e.target.value }));
  };

  // Обработка изменений баланса токенов — для простоты сделаем строки ключ-значение
  const onBalanceChange = (token: string, value: string) => {
    const numValue = Number(value);
    setEditData((prev) => ({
      ...prev,
      balance: {
        ...prev.balance,
        [token]: isNaN(numValue) ? 0 : numValue,
      },
    }));
  };

  const saveChanges = async () => {
    if (editingUserId === null) return;

    await updateUser({
      id: editingUserId,
      login: editData.login,
      password: undefined,
      balance: editData.balance,
    });
    setEditingUserId(null);
  };

  // Отмена редактирования
  const cancelEdit = () => {
    setEditingUserId(null);
  };

  return (
    <div>
      <h2>Пользователи</h2>
      <ul>
        {users.map((user) => (
          <li key={user.id} style={{ marginBottom: "1rem", borderBottom: "1px solid #ccc" }}>
            {editingUserId === user.id ? (
              <div>
                <input
                  type="text"
                  value={editData.login}
                  onChange={onLoginChange}
                  placeholder="Логин"
                />
                <div>
                  Баланс токенов:
                  {Object.entries(editData.balance).map(([token, amount]) => (
                    <div key={token}>
                      <label>{token}: </label>
                      <input
                        type="number"
                        value={amount}
                        onChange={(e) => onBalanceChange(token, e.target.value)}
                        min={0}
                      />
                    </div>
                  ))}
                </div>
                {/* Для добавления нового токена */}
                <AddTokenInput
                  currentTokens={Object.keys(editData.balance)}
                  onAdd={(token) =>
                    setEditData((prev) => ({
                      ...prev,
                      balance: { ...prev.balance, [token]: 0 },
                    }))
                  }
                />
                <button onClick={saveChanges}>Сохранить</button>
                <button onClick={cancelEdit}>Отмена</button>
              </div>
            ) : (
              <div>
                <strong>{user.login}</strong> Баланс:{" "}
                {Object.entries(user.balance)
                  .map(([token, amount]) => `${token}: ${amount}`)
                  .join(", ")}
                <button style={{ marginLeft: "1rem" }} onClick={() => startEdit(user)}>
                  Редактировать
                </button>
              </div>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
};

// Компонент для добавления нового токена в баланс
const AddTokenInput: React.FC<{
  currentTokens: string[];
  onAdd: (token: string) => void;
}> = ({ currentTokens, onAdd }) => {
  const [newToken, setNewToken] = useState("");

  const onAddClick = () => {
    const trimmed = newToken.trim();
    if (trimmed && !currentTokens.includes(trimmed)) {
      onAdd(trimmed);
      setNewToken("");
    }
  };

  return (
    <div style={{ marginTop: "0.5rem" }}>
      <input
        type="text"
        value={newToken}
        onChange={(e) => setNewToken(e.target.value)}
        placeholder="Добавить новый токен"
      />
      <button onClick={onAddClick}>Добавить</button>
    </div>
  );
};

export default UsersList;