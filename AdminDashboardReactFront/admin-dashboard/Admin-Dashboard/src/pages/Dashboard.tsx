import React, { useEffect, useState } from "react";
import axios from "axios";
import type { User } from "../types/User";
import { useAdmin } from '../context/AdminContext';
import type { Token } from "../types/Token";

const Dashboard: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [editingUserId, setEditingUserId] = useState<number | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const { updateUser } = useAdmin();

  const [tokenRates, setTokenRates] = useState<Token[]>([]);
  const [editingTokens, setEditingTokens] = useState<{ [key: string]: Token }>({});

  // Загрузка пользователей
  useEffect(() => {
    loadUsers();
  }, []);

  // Загрузка курсов токенов
  useEffect(() => {
    loadTokenRates();
  }, []);

  async function loadUsers() {
    try {
      const res = await axios.get<User[]>("http://localhost:5000/clients", { withCredentials: true });
      setUsers(res.data);
    } catch (e) {
      console.error(e);
    }
  }

  async function loadTokenRates() {
    try {
      const res = await axios.get<Token[]>("http://localhost:5000/rate/all", { withCredentials: true });
      setTokenRates(res.data);
    } catch (e) {
      console.error(e);
    }
  }

  const handleDeleteUser = async (userId: number) => {
    if (!window.confirm("Вы уверены, что хотите удалить этого пользователя?" + userId)) return;

    try {
      await axios.delete(`http://localhost:5000/user/delete?id=${userId}`, { withCredentials: true });
      setUsers((prevUsers) => prevUsers.filter((user) => user.id !== userId));
    } catch (e) {
      console.error("Ошибка при удалении пользователя", e);
    }
  };

  const startEditingToken = (token: Token) => {
    setEditingTokens((prev) => ({ ...prev, [token.id]: { ...token } }));
  };

  const cancelEditingToken = (tokenId: number) => {
    setEditingTokens((prev) => {
      const copy = { ...prev };
      delete copy[tokenId];
      return copy;
    });
  };

  const handleChangeToken = (
    tokenId: number,
    field: keyof Token,
    value: string | number
  ) => {
    setEditingTokens((prev) => ({
      ...prev,
      [tokenId]: { ...prev[tokenId], [field]: field === "rate" ? Number(value) : value },
    }));
  };

  const handleSaveToken = async (tokenId: number) => {
    const editedToken = editingTokens[tokenId];
    if (!editedToken) return;

    try {
      await axios.put(`http://localhost:5000/rate/update?id=${tokenId}`, editedToken);

      setTokenRates((prev) =>
        prev.map((token) =>
          token.id === tokenId ? editedToken : token
        )
      );

      cancelEditingToken(tokenId);
    } catch (e) {
      console.error("Ошибка при сохранении токена", e);
      alert("Не удалось сохранить изменения.");
    }
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement>,
    userId: number,
    type?: "token" | "payment",
    itemId?: number
  ) => {
    const { name, value } = e.target;
    setUsers((prevUsers) =>
      prevUsers.map((user) => {
        if (user.id !== userId) {
          return user;
        }
        if (name === "email") {
          return { ...user, email: value };
        }
        if (name === "login") {
          return { ...user, login: value };
        }
        else if (type === "token" && itemId !== undefined) {
          const updatedTokens = user.tokenBalance.map((token) =>
            token.id === itemId ? { ...token, countToken: parseInt(value) } : token
          );
          return { ...user, tokenBalance: updatedTokens };
        }
        else if (type === "payment" && itemId !== undefined) {
          const updatedPayments = user.payments.map((p) =>
            p.id === itemId ? { ...p, [name]: name === "paymentCost" ? parseFloat(value) : value } : p
          );
          return { ...user, payments: updatedPayments };
        }
        return user;
      })
    );
  };

  const handleSave = async (user: User) => {
    const success = await updateUser(user);
    if (success) {
      setEditingUserId(null);  // Выходим из режима редактирования
      await loadUsers();       // Обновляем список пользователей
    } else {
      alert("Не удалось сохранить изменения");
    }
  };

  return (
    <div>
      {/* Таблица курсов токенов */}
      <div className="mb-6 p-4 border rounded shadow-sm max-w-md">
        <h2 className="text-xl font-semibold mb-2">Курс токенов</h2>
        <table className="w-full border-collapse border border-gray-300 text-left">
          <thead>
            <tr className="bg-gray-100">
              <th className="border p-2">Действия</th>
              <th className="border p-2">Token Name</th>
              <th className="border p-2">Rate (₽)</th>
            </tr>
          </thead>
          <tbody>
            {tokenRates.map(({ id, nameToken, rate }) => {
              const isEditingToken = editingTokens.hasOwnProperty(id);
              const editedToken = editingTokens[id] || { id, nameToken, rate };

              return (
                <tr key={id}>
                  <td className="border p-2">
                    {isEditingToken ? (
                      <>
                        <button
                          onClick={() => handleSaveToken(id)}
                          className="mr-2 px-3 py-1 bg-green-500 text-white rounded"
                        >
                          Сохранить
                        </button>
                        <button
                          onClick={() => cancelEditingToken(id)}
                          className="px-3 py-1 bg-gray-300 rounded"
                        >
                          Отмена
                        </button>
                      </>
                    ) : (
                      <button
                        onClick={() => startEditingToken({ id, nameToken, rate })}
                        className="px-3 py-1 bg-blue-500 text-white rounded"
                      >
                        Редактировать
                      </button>
                    )}
                  </td>
                  <td className="border p-2">
                    {isEditingToken ? (
                      <input
                        type="text"
                        value={editedToken.nameToken}
                        onChange={(e) =>
                          handleChangeToken(id, "nameToken", e.target.value)
                        }
                      />
                    ) : (
                      nameToken
                    )}
                  </td>
                  <td className="border p-2">
                    {isEditingToken ? (
                      <input
                        type="number"
                        step="0.01"
                        value={editedToken.rate}
                        onChange={(e) =>
                          handleChangeToken(id, "rate", e.target.value)
                        }
                      />
                    ) : (
                      rate
                    )}
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>

      {/* Таблица пользователей */}
      <div className="p-6 max-w-full overflow-x-auto">
        <table className="w-full border-collapse border border-gray-300">
          <thead>
            <tr className="bg-gray-100">
              <th className="border p-2">ID</th>
              <th className="border p-2">Login</th>
              <th className="border p-2">Email</th>
              <th className="border p-2">Tokens</th>
              <th className="border p-2">Payments</th>
              <th className="border p-2">Actions</th>
            </tr>
          </thead>
          <tbody>
            {users.map((user) => (
              <tr key={user.id}>
                <td className="border p-2 align-top">{user.id}</td>
                <td className="border p-2 align-top">
                  <input
                    name="login"
                    value={user.login}
                    onChange={(e) => handleChange(e, user.id)}
                    disabled={!isEditing}
                  />
                </td>
                <td className="border p-2 align-top">
                  <input
                    name="email"
                    value={user.email}
                    onChange={(e) => handleChange(e, user.id)}
                    disabled={!isEditing}
                  />
                </td>
                <td className="border p-2 align-top max-w-xs overflow-auto">
                  <div className="max-h-40 overflow-y-auto">
                    {user.tokenBalance.map((token) => (
                      <div key={token.id}>
                        <label>#{token.id}: </label>
                        <input
                          name="countToken"
                          type="number"
                          value={token.countToken}
                          onChange={(e) => handleChange(e, user.id, "token", token.id)}
                          disabled={!isEditing}
                          className="w-16"
                        />
                        <input
                          name="nameToken"
                          value={token.token.nameToken}
                          disabled
                          className="w-16"
                        />
                      </div>
                    ))}
                  </div>
                </td>
                <td className="border p-2 align-top max-w-xs overflow-auto">
                  <div className="max-h-40 overflow-y-auto">
                    {user.payments.map((payment) => (
                      <div key={payment.id} className="mb-2 border-b pb-1">
                        <input
                          name="paymentName"
                          value={payment.paymentName}
                          onChange={(e) => handleChange(e, user.id, "payment", payment.id)}
                          disabled={!isEditing}
                          placeholder="Name"
                        />
                        <input
                          name="paymentCost"
                          type="number"
                          value={payment.paymentCost}
                          onChange={(e) => handleChange(e, user.id, "payment", payment.id)}
                          disabled={!isEditing}
                          placeholder="Cost"
                          className="w-20"
                        />
                        <input
                          name="dateOfPurchase"
                          type="date"
                          value={payment.dateOfPurchase.split("T")[0]}
                          onChange={(e) => handleChange(e, user.id, "payment", payment.id)}
                          disabled={!isEditing}
                        />
                      </div>
                    ))}
                  </div>
                </td>
                <td className="border p-2 align-top">
                  {isEditing ? (
                    <>
                      <button onClick={() => handleSave(user)}>Сохранить</button>
                      <button onClick={() => setIsEditing(false)}>Отмена</button>
                    </>
                  ) : (
                    <>
                      <button onClick={() => setIsEditing(true)}>Редактировать</button>
                      <button
                        onClick={() => handleDeleteUser(user.id)}
                        className="ml-2 text-red-600 hover:text-red-900"
                      >
                        Удалить
                      </button>
                    </>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default Dashboard;
