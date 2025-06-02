module.exports = {
    apps: [
      {
        name: "orm",
        script: "dotnet",
        args: [
          "run",
          "--configuration", "Release",
          "--launch-profile", "http",
          "--project",
          "ORMConvertorAPI/ORMConvertorAPI.csproj"
        ],
        time: true,
        watch: false,
        env: {
          version: "",
          Version: ""
        }
      }
    ]
  };
  