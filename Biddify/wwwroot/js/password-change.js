function sendOtp() {
    const email = document.getElementById("emailInput").value.trim();
    if (!email) {
        alert("Please enter your email.");
        return;
    }

    fetch("?handler=SendOtp", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email })
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                alert("OTP sent to " + email);
                document.getElementById("otpSection").classList.remove("hidden");
            } else {
                alert("Failed to send OTP.");
            }
        })
        .catch(err => {
            console.error(err);
            alert("An error occurred while sending OTP.");
        });
}

function savePassword() {
    const email = document.getElementById("emailInput").value.trim();
    const otp = document.getElementById("otpInput").value.trim();
    const newPass = document.getElementById("newPassword").value.trim();
    const confirmPass = document.getElementById("confirmPassword").value.trim();

    if (!otp || !newPass || !confirmPass) {
        alert("Please fill in all fields.");
        return;
    }

    if (newPass !== confirmPass) {
        alert("Passwords do not match.");
        return;
    }

    fetch("?handler=ChangePassword", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, otp, newPassword: newPass })
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                alert("Password changed successfully!");
                window.location.reload();
            } else {
                alert(data.message || "Failed to change password.");
            }
        })
        .catch(err => {
            console.error(err);
            alert("An error occurred while changing password.");
        });
}
