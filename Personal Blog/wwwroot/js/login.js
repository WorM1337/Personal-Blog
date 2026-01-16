const loginForm = document.getElementById("loginForm");
const errorMessage = document.getElementById("errorMessage");

loginForm.addEventListener("submit", async function (event) {
  event.preventDefault();

  const login = document.getElementById("username").value;
  const password = document.getElementById("password").value;

  errorMessage.style.display = "none";

  try {
    const response = await fetch("http://localhost:5000/api/auth/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        login: login,
        password: password,
      }),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "Login failed");
    }

    const data = await response.json();

    if (data.token || data.accessToken) {
      const token = data.token || data.accessToken;
      localStorage.setItem("jwtToken", token);

      if (data.user) {
        localStorage.setItem("user", JSON.stringify(data.user));
      }

      window.location.href = "./admin.html";
    } else {
      throw new Error("Token not found in response");
    }
  } catch (error) {
    errorMessage.textContent =
      error.message || "An error occurred during login";
    errorMessage.style.display = "block";
    console.error("Login error:", error);
  }
});
